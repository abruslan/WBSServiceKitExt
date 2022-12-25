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
    public class WBS_SyncLogItem : BaseEditedEntity
    {
        [Display(Name = "Проект")]
        public WBS_SyncLog SyncLog { get; set; }

        [ForeignKey(nameof(SyncLog))]
        public Guid? WBS_SyncLogId { get; set; }

        [Display(Name = "Система")]
        [ForeignKey(nameof(SyncSystemId))]
        public WBS_SyncSystem SyncSystem { get; set; }

        [Display(Name = "Система")]
        public Guid? SyncSystemId { get; set; }

        [Display(Name = "Сообщение")]
        public string Message { get; set; }

        [Display(Name = "Уровень значимости")]
        public int Level { get; set; }
    }
}
