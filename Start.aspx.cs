using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Configuration;

namespace ZohoIntegration
{
    public partial class Start : System.Web.UI.Page
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
            log.Info("Started Page Load");
            lblError.Text = "";
            try
            {
                ZohoBookAuthCodeUrl = ConfigurationManager.AppSettings["ZohoBook-AuthCode-Url"];
                ZohoBookAccessTokenUrl = ConfigurationManager.AppSettings["ZohoBook-AccessToken-Url"];
                ZohoBookRefreshTokenUrl = ConfigurationManager.AppSettings["ZohoBook-RefreshToken-Url"];
                ZohoBookClientCode = ConfigurationManager.AppSettings["ZohoBook-ClientCode"];
                ZohoBookClientSecret = ConfigurationManager.AppSettings["ZohoBook-ClientSecret"];
                ZohoBookItemsScope = ConfigurationManager.AppSettings["ZohoBook-ItemsScope"];
                ZohoBookRedirectUrl = ConfigurationManager.AppSettings["ZohoBook-RedirectUrl"];
                ZohoBookOrgId = ConfigurationManager.AppSettings["ZohoBook-OrgId"];

                if (!IsPostBack)
                {
                    //Check if code is available in querystring
                    if (Request.QueryString["code"] != null)
                    {
                        log.Info("Access Token Flow. Auth Code is - " + Request.QueryString["code"].ToString());
                        //Access Token flow
                        string code = Request.QueryString["code"].ToString();
                        string authUrl = string.Concat(ZohoBookAccessTokenUrl,
                            "?grant_type=authorization_code&client_id={0}&client_secret={1}&redirect_uri={2}&code={3}");


                        ApiModel model = new ApiModel();
                        model.RelativeUrl = string.Format(authUrl, ZohoBookClientCode, ZohoBookClientSecret, ZohoBookRedirectUrl, code);
                        model.Method = HttpMethod.Post;

                        log.Info("Auth Url - " + model.RelativeUrl);

                        var response = ApiHelper.ExecuteAPI(model);

                        if (response != null && response.IsSuccessStatusCode)
                        {
                            var jsonResponse = response.Content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(jsonResponse);

                            Session["Zoho-AccessToken"] = jobject.ContainsKey("access_token") ? jobject["access_token"].ToString() : "";
                            Session["Zoho-RefreshToken"] = jobject.ContainsKey("refresh_token") ? jobject["refresh_token"].ToString() : "";

                            UpdateConfig("ZohoBook-RefreshToken", Session["Zoho-RefreshToken"].ToString());

                            log.Info("Response From Access Token API - " + jsonResponse);

                            btnAuthenticate.Enabled = false;
                            Response.Redirect("ZohoImport.aspx?auth=success");
                        }
                        else
                        {
                            var jsonResponse = response.Content.ReadAsStringAsync().Result;

                            log.Info("Error From Access Token API - " + jsonResponse);
                        }
                    }
                    else if (Request.QueryString["auth"] != null && Request.QueryString["auth"].ToString() == "success")
                    {
                        lblError.Text = "Successfully Authenticated";
                        btnAuthenticate.Enabled = false;
                    }
                    else if (Session["Zoho-AccessToken"] != null)
                    {
                        btnAuthenticate.Enabled = false;
                    }
                    else
                    {
                        //Auth Code flow
                        //It should be triggered from Authenticate button
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception - " + ex.ToString());
            }

            log.Info("Completed Page Load");
        }

        protected void btnAuthenticate_Click(object sender, EventArgs e)
        {
            log.Info("Authenticate Started");

            string authUrl = string.Concat(ZohoBookAuthCodeUrl,
                "?response_type=code&client_id={0}&scope={1}&redirect_uri={2}&access_type=offline");
            string authCodeUrl = string.Format(authUrl, ZohoBookClientCode, ZohoBookItemsScope, ZohoBookRedirectUrl);

            log.Info("Auth Code Url - " + authCodeUrl);

            Response.Redirect(authCodeUrl);
        }

        private void UpdateConfig(string key, string value)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings[key].Value = value;
            config.Save();
        }
    }
}