using MHRSLiteBusiness.Contracts;
using MHRSLiteDataAccess;
using MHRSLiteEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusiness.Implementations
{
    public class AppointmentHourRepository
        : Repository<AppointmentHour>, IAppointmentHourRepository
    {
        
        public AppointmentHourRepository(MyContext myContext)
            : base(myContext)
        {
           
        }
    }
}
