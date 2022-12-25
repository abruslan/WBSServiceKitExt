using ServiceKit.ExternalSystem.Common;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors.PayDox
{
    public class PayDoxConfiguration : IExternalSystemConfiguration
    {
        private readonly WBS_SyncSystem _SyncSystem;
        public Guid? SourceId => _SyncSystem.Id;
        public PayDoxConfiguration(WBS_SyncSystem SyncSystem)
        {
            _SyncSystem = SyncSystem;
            Name = SyncSystem.Name;
        }

        public string Name { get; set; }

        public string GetConnectionString()
        {
            return _SyncSystem.ConnectionString;
        }
    }
}
