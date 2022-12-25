using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceKit.Quotes
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        public string Tags { get; set; }
    }
}
