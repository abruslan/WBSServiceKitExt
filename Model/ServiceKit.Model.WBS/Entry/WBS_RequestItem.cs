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
   public class WBS_RequestItem : BaseEditedEntity
    {
        [Display(Name = "Заявка")]
        public WBS_Request Parent { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        [Display(Name = "Код ИСР")]
        public string Code{ get; set; }
        [Display(Name = "Наименование измененного/удаленного объекта")]
        public string Name { get; set; }
        [Display(Name = "Примечание")]
        public string Comment { get; set; }
        [Display(Name = "Новый код ИСР")]
        public string NewCode { get; set; }
        [Display(Name = "Новое наименование")]
        public string NewName{ get; set; }

        [Display(Name = "Полное имя в 1C")]
        public string Full1CName { get; set; }


        public Guid? TreeItemId { get; set; }

        [Display(Name = "Ошибка")]
        public string ErrorMessage { get; set; }
    }
}
