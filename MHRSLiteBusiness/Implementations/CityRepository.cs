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
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly MyContext _myContext;

        public CityRepository(MyContext myContext)
            : base(myContext)
        {

        }
    }
}
