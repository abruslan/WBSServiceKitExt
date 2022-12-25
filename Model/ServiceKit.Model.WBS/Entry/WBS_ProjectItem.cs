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
    //[Table(nameof(WBS_RequestItem), Schema = Common.Schema)]
   public class WBS_ProjectItem : BaseEditedEntity
    {
        [Display(Name = "Проект")]
        public WBS_Project Project { get; set; }

        [ForeignKey(nameof(Project))]
        public Guid? ProjectId { get; set; }
        
        [Display(Name = "Родительский узел")]
        public Guid? ParentId { get; set; }

        [Display(Name = "Уровень")]
        public int Level { get; set; }

        [Display(Name = "№ п/п")]
        public int OrderNum { get; set; }

        [Display(Name = "Код")]
        public string ShortCode { get; set; }
        [Display(Name = "Полный код")]
        public string FullCode { get; set; }

        [Display(Name = "Наименование")]
        public string ShortName { get; set; }
        
        [Display(Name = "Полное наименование в 1С")]
        public string Full1CName { get; set; }

        [Display(Name = "Примечание")]
        public string Comment { get; set; }
    }
}
