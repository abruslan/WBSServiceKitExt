using ServiceKit.Helpers.Common;
using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    /// <summary>
    /// Заказчик / Договор
    /// </summary>
    public class Client : BaseEditedEntity
    {
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Полное наименование")]
        public string FullName { get; set; }
        [Display(Name = "ИНН")]
        public string INN { get; set; }
        [Display(Name = "Договор (для отчета)")]
        public string Contract { get; set; }

        public string GetName() => string.IsNullOrEmpty(FullName?.Trim()) ? Name : FullName;

        public string GetFileName() => Name.RemoveInvalidFileNameChars();

    }
}
