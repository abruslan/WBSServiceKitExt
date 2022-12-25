using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.ExternalSystem.Common
{
    public class WebServiceEx
    {
        private readonly IWebServiceConfiguration _configuration;

        public WebServiceEx(IWebServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        private ICredentials Credentials
            => new NetworkCredential(_configuration.Login, _configuration.Password);

        public T UploadJson<T>(string operation, string data)
        {
            using (var wc = new WebClientEx { Credentials = Credentials, Encoding = Encoding.UTF8 })
            {
                wc.Timeout = 30 * 60 * 1000;
                var json = wc.UploadString(_configuration.BaseURL + operation, data);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public T DownloadJson<T>(string operation)
        {
            using (var wc = new WebClientEx { Credentials = Credentials, Encoding = Encoding.UTF8 })
            {
                wc.Timeout = 30 * 60 * 1000;
                var json = wc.DownloadString(_configuration.BaseURL + operation);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

    }
}
