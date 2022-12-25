using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.WBS.Entry
{
    public class WBS_ProjectPublication : BaseEditedEntity
    {
        [Display(Name = "Проект")]
        public WBS_Project Project { get; set; }

        [ForeignKey(nameof(Project))]
        public Guid? ProjectId { get; set; }

        [Display(Name = "Внешняя система")]
        public WBS_SyncSystem SyncSystem { get; set; }

        [ForeignKey(nameof(SyncSystem))]
        public Guid? SyncSystemId { get; set; }
    }
}
