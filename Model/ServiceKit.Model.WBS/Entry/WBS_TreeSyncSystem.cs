using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.Entry
{
    public class WBS_TreeSyncSystem : BaseEntity, IEntity
    {
        public Guid ResearchId { get; set; }
        public WBS_Item TreeItem { get; set; }

        public Guid SyncSystemId { get; set; }
        public WBS_SyncSystem SyncSystem { get; set; }

    }
}
