using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.ExternalSystem.Common
{
    public interface IExternalSystemConfiguration
    {
        public string Name { get; set; }
        public Guid? SourceId { get; }
        public string GetConnectionString();
    }
}
