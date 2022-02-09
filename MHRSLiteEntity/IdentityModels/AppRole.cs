using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntity.IdentityModels
{
    public class AppRole:IdentityRole
    {
        [StringLength(400,ErrorMessage ="Role Açıklamasına En Fazla 400 Karakter Girilebilir.")]
        public string Description { get; set; }

    }
}
