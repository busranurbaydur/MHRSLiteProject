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
    public class AppointmentRepository : Repository<Appointment>,
          IAppointmentRepository
    {
       
        public AppointmentRepository(MyContext myContext) : base(myContext)
        {
           
        }
    }
}
