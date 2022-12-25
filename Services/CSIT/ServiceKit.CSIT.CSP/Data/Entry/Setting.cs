using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    public class Setting : BaseEditedEntity
    {
        [Display(Name = "Подписант")]
        public Signer Signer { get; set; }
    }
}
