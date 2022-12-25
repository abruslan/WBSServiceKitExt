using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    public class Signer : BaseEditedEntity
    {
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "ФИО")]
        public string FullName { get; set; }
        [Display(Name = "Должность")]
        public string Position { get; set; }
        [Display(Name = "Компания")]
        public string Company { get; set; }
    }
}
