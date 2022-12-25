using ServiceKit.ExternalSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Configuration
{
    public class OneCConfiguration : IWebServiceConfiguration
    {
        public string BaseURL { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
