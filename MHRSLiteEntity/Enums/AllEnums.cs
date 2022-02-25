using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntity.Enums
{
    public class AllEnums
    {
    }

    public enum Genders : byte
    {
        Belirtilmemiş,
        Bay,
        Bayan
    }

    public enum RoleNames : byte
    {
        Passive,
        Admin,
        Patient,
        PassiveDoctor,
        ActiveDoctor
    }

    public enum AppointmentStatus : byte
    {
        Active,
        Past,
        Cansel
    }
}
