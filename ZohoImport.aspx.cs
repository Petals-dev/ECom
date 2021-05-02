using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Configuration;

namespace ZohoIntegration
{
    public partial class ZohoImport : System.Web.UI.Page
    {
        string ZohoBookAuthCodeUrl = string.Empty;
        string ZohoBookAccessTokenUrl = string.Empty;
        string ZohoBookRefreshTokenUrl = string.Empty;
        string ZohoBookClientCode = string.Empty;
        string ZohoBookClientSecret = string.Empty;
        string ZohoBookItemsScope = string.Empty;
        string ZohoBookRedirectUrl = string.Empty;
        string ZohoBookOrgId = string.Empty;

        ILog log = log4net.LogManager.GetLogger(typeof(ZohoImport));

        protected void Page_Load(object sender, EventArgs e)
        {
           
                ZohoBookAuthCodeUrl = ConfigurationManager.AppSettings["ZohoBook-AuthCode-Url"];
                ZohoBookAccessTokenUrl = ConfigurationManager.AppSettings["ZohoBook-AccessToken-Url"];
                ZohoBookRefreshTokenUrl = ConfigurationManager.AppSettings["ZohoBook-RefreshToken-Url"];
                ZohoBookClientCode = ConfigurationManager.AppSettings["ZohoBook-ClientCode"];
                ZohoBookClientSecret = ConfigurationManager.AppSettings["ZohoBook-ClientSecret"];
                ZohoBookItemsScope = ConfigurationManager.AppSettings["ZohoBook-ItemsScope"];
                ZohoBookRedirectUrl = ConfigurationManager.AppSettings["ZohoBook-RedirectUrl"];
                ZohoBookOrgId = ConfigurationManager.AppSettings["ZohoBook-OrgId"];
            
        }

        private void UpdateConfig(string key, string value)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings[key].Value = value;
            config.Save();            
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
           bool isImportSuccess= ImportItems();

            if (isImportSuccess == false)
            {
                //Get token from refresh token
                GetNewToken();

                ImportItems();
            }
        }

        private bool ImportItems()
        {
            log.Info("Import Started");

            try
            {
                if (Session["Zoho-AccessToken"] != null)
                {
                    int pageSize = 200;
                    int pageIndex = 1;
                    bool hasMorePages = true;

                    List<ItemsModel> gridItems = new List<ItemsModel>();
                    while (hasMorePages)
                    {
                        //Access Token found
                        string itemsUrl = string.Format(ConfigurationManager.AppSettings["ZohoBook-Items"] + "organization_id={0}", ZohoBookOrgId);

                        itemsUrl = string.Format(itemsUrl + "&page={0}&per_page={1}", pageIndex, pageSize);

                        log.Info("Items Url - " + itemsUrl);

                        ApiModel model = new ApiModel();
                        model.RelativeUrl = itemsUrl;
                        model.Method = HttpMethod.Get;

                        model.Headers = new Dictionary<string, string>();
                        model.Headers.Add("Authorization", "Bearer " + Session["Zoho-AccessToken"]);


                        var response = ApiHelper.ExecuteAPI(model);

                        if (response != null && response.IsSuccessStatusCode)
                        {
                            var jsonResponse = response.Content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(jsonResponse);

                            log.Info("Items Data - " + jsonResponse);

                            JArray itemArray = (JArray)jobject["items"];

                            if (!(bool)jobject["page_context"]["has_more_page"])
                            {
                                hasMorePages = false;
                            }

                            List<ItemsModel> items = new List<ItemsModel>();
                            SqlHelper sqlHelper = new SqlHelper();

                            foreach (JObject item in itemArray)
                            {
                                ItemsModel itemModel = new ItemsModel();
                                itemModel.ItemId = item["item_id"].ToString();
                                itemModel.Name = item["name"].ToString();
                                itemModel.Unit = item["unit"].ToString();
                                itemModel.Description = item["description"].ToString();
                                itemModel.ItemType = item["item_type"].ToString();
                                itemModel.AvailableStock = item["available_stock"].ToString();
                                itemModel.SKU = item["sku"].ToString();
                                itemModel.Category = item["cf_master_category"].ToString();
                                itemModel.SubCategory = item["cf_sub_category"].ToString();
                                itemModel.SellingPrice = item["rate"].ToString();
                                itemModel.StockOnHand = item["stock_on_hand"].ToString();
                                itemModel.Status = item["status"].ToString();
                                itemModel.Brand = item["cf_brand"].ToString();

                                items.Add(itemModel);

                                sqlHelper.ImportData(itemModel);
                            }

                            gridItems.AddRange(items);

                            sqlHelper.CloseConnection();

                            lblError.Text = "Import Completed";
                            log.Info("Import Completed");
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            lblError.Text = "Token Expired. Please Re-Authenticate";
                                                   

                            return false;
                        }
                        else
                        {
                            var jsonResponse = response.Content.ReadAsStringAsync().Result;
                            log.Info("Error from Items API - " + jsonResponse);

                            lblError.Text = jsonResponse;
                            return false;
                        }
                        pageIndex = pageIndex + 1;
                    }

                    grdItems.DataSource = gridItems;
                    grdItems.DataBind();
                }
                else
                {
                    lblError.Text = "Token Expired. Please Autehnticate";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception - " + ex.ToString());
                lblError.Text = ex.Message;
                return false;
            }
        }


        private string GetNewToken()
        {
            string refreshToken = ConfigurationManager.AppSettings["ZohoBook-RefreshToken"];

            string authUrl = string.Concat(ZohoBookRefreshTokenUrl,
                            "?grant_type=refresh_token&client_id={0}&client_secret={1}&refresh_token={2}");

            ApiModel model = new ApiModel();
            model.RelativeUrl = string.Format(authUrl, ZohoBookClientCode, ZohoBookClientSecret, refreshToken);
            model.Method = HttpMethod.Post;

            var response = ApiHelper.ExecuteAPI(model);

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                JObject jobject = JObject.Parse(jsonResponse);

                Session["Zoho-AccessToken"] = jobject.ContainsKey("access_token") ? jobject["access_token"].ToString() : "";                

                log.Info("Response From Refresh Token API - " + jsonResponse);
               
                return Session["Zoho-AccessToken"].ToString();
            }
            else
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                log.Info("Error From Access Token API - " + jsonResponse);
                return string.Empty;
            }
        }
    }
}