using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    /// <summary>
    /// Приложения к договору
    /// </summary>
    public class ClientAnnex : BaseEditedEntity
    {
        [Display(Name = "Клиент")]
        public Client Client { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Действует с")]
        public DateTime DateFrom { get; set; }
        
        [Display(Name = "Действует по")]
        public DateTime DateTo { get; set; }
    }
}
