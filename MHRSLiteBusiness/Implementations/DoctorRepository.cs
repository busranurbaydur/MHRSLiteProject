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
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
       

        public DoctorRepository(MyContext myContext)
            : base(myContext)
        {

        }

    
    }
}
