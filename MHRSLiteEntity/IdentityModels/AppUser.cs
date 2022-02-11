using MHRSLiteEntity.Enums;
using MHRSLiteEntity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntity.IdentityModels
{
    public class AppUser:IdentityUser
    {
        [StringLength(50,MinimumLength =2,ErrorMessage ="Adınız en az 2, en çok 50 karakter olmalıdır..")]
        [Required(ErrorMessage ="Adınız Gereklidir..")]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyadınız en az 2, en çok 50 karakter olmalıdır..")]
        [Required(ErrorMessage = "Soyadınız Gereklidir..")]
        public string Surname { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public string Picture { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Lütfen Cinsiyetinizi Seçiniz..")]
        public Genders Gender { get; set; }

        //doktor tab ilişki kuruldu
        public virtual List<Doctor> Doctors { get; set; }

        //hasta tab ilişki kuruldu
        public virtual List<Patient> Patients { get; set; }

    }
}
