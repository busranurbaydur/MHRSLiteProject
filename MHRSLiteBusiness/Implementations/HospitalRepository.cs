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
    public class HospitalRepository : Repository<Hospital>, IHospitalRepository
    {
       
        public HospitalRepository(MyContext myContext) : base(myContext)
        {
            
        }
    }
}

