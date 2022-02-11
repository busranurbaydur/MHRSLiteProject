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
    public class DistrictRepository:Repository<District>, IDistrictRepository
    {
        private readonly MyContext _myContext;

        public DistrictRepository(MyContext myContext)
            :base(myContext)
        {

        }

    }
}
