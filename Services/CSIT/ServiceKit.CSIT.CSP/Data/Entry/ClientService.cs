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
    /// Услуги по заказчикам
    /// </summary>
    public class ClientService : BaseEditedEntity
    {
        [Display(Name = "Наименование")]
        public Client Client { get; set; }
        [Display(Name = "Наименование в реестре")]
        public string OriginalName { get; set; }

        [Display(Name = "Приложение")]
        public ClientAnnex ClientAnnex { get; set; }

        [Display(Name = "Наименование в отчете")]
        public string Name { get; set; }
        [Display(Name = "Цена, руб./мес.")]
        public decimal Price { get; set; }
        [Display(Name = "Цена, руб./год.")]
        public decimal YearPrice { get; set; }
    }
}
