using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Model.Common
{
    public class BaseEditedEntity : BaseEntity, IEntity
    {
        /// <summary>
        /// Признак удаления
        /// </summary>
        [Display(Name = "Признак удаления")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Кто создал
        /// </summary>
        [Display(Name = "Кто создал")]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Дата/время создания
        /// </summary>
        [Display(Name = "Дата/время создания")]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Кто изменил
        /// </summary>
        [Display(Name = "Кто изменил")]
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Дата/время изменения
        /// </summary>
        [Display(Name = "Дата/время изменения")]
        public DateTime? Modified { get; set; }
    }
}
