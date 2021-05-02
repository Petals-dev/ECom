using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace ZohoIntegration
{
    public class ApiModel
    {
        public string BaseUrl { get; set; }

        public string RelativeUrl { get; set; }

        public string JsonContent { get; set; }

        public Dictionary<string,string> Headers { get; set; }

        public HttpMethod Method  { get; set; }
    }
}