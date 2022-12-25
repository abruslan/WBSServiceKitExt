using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    /// <summary>
    /// Файл реестра. Основная сущность
    /// </summary>
    public class Register : BaseEditedEntity
    {
        [Display(Name = "Статус")]
        public RegisterStatus Status { get; set; }
        [Display(Name = "Исполнитель")]
        public string Contractor { get; set; }
        [Display(Name = "ИНН исполнителя")]
        public string ContractorINN { get; set; }
        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }
        [Display(Name = "ИНН заказчика")]
        public string CustomerINN { get; set; }
        [Display(Name = "Заказчик")]
        public Company Customer { get; set; }

        [Display(Name = "Счет")]
        public string Account { get; set; }
        [Display(Name = "Дата счета")]
        public DateTime AccountDate { get; set; }
        [Display(Name = "Накладная / Акт №")]
        public string Invoice { get; set; }
        [Display(Name = "Валюта")]
        public string Currency { get; set; }
        [Display(Name = "Договор")]
        public string Contract { get; set; }
        [Display(Name = "Период продажи (с)")]
        public DateTime SaleFrom { get; set; }
        [Display(Name = "Период продажи (по)")]
        public DateTime SaleTo { get; set; }

        public string FileName { get; set; }
        public long FileLength { get; set; }
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        public virtual List<RegisterItem> Items { get; set; }
    }
}
