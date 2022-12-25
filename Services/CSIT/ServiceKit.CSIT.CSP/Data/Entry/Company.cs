using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data.Entry
{
    /// <summary>
    /// Компания, на которую выставляются услуги в реестре
    /// CS IT или Абилити
    /// </summary>
    public class Company : BaseEditedEntity
    {
        public string Name { get; set; }
        public string INN { get; set; }
    }
}
