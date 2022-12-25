using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.ExternalSystem.Common
{
    public interface IWebServiceConfiguration
    {
        string BaseURL { get; }
        string Login { get; }
        string Password { get; }
    }
}
