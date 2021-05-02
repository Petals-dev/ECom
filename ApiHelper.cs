using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ZohoIntegration
{
    public static class ApiHelper
    {
        public static HttpResponseMessage ExecuteAPI(ApiModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();

            if (!string.IsNullOrEmpty(model.BaseUrl))
            {
                client.BaseAddress = new Uri(model.BaseUrl);
            }
            
            if(model.Headers!=null && model.Headers.Count > 0)
            {
                client.DefaultRequestHeaders.Clear();

                foreach(KeyValuePair<string,string> header in model.Headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (model.Method == HttpMethod.Get)
            {
                response= client.GetAsync(model.RelativeUrl).Result;
            }
            else if (model.Method == HttpMethod.Post)
            {
                StringContent httpContent = string.IsNullOrEmpty(model.JsonContent) ? new StringContent(string.Empty) : new StringContent(model.JsonContent);

                response = client.PostAsync(model.RelativeUrl, httpContent).Result;
            }

            return response;
        }
    }
}