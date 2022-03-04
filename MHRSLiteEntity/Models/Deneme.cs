using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntity.Models
{
    public class Deneme : Base<int>
    {
        [Required]
        [StringLength(150, ErrorMessage = "En fazla 150 karakter olmalı")]
        public string Mesaj { get; set; }

    }
}
