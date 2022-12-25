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
   public class WBS_RequestProjectItem : BaseEditedEntity
    {
        [Display(Name = "Заявка")]
        public WBS_Request Request { get; set; }

        [ForeignKey(nameof(Request))]
        public Guid? RequestId { get; set; }

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

        [Display(Name = "Примечание")]
        public string Comment { get; set; }

        [Display(Name = "Ошибка")]
        public string ErrorMessage { get; set; }

        [Display(Name = "Полное имя в 1C")]
        public string Full1CName { get; set; }




        public bool hasError() => !string.IsNullOrEmpty(ErrorMessage);
    }
}
