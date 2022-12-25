using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.ExternalSystem.Common
{
    public class WebClientEx : WebClient
    {
        public int Timeout { get; set; } = 10 * 60 * 1000;
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = Timeout;
            return w;
        }
    }
}
