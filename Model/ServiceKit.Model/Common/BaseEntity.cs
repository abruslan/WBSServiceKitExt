using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.Common
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
