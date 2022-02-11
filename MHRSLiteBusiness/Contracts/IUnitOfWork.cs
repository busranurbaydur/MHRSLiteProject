using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusiness.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        ICityRepository CityRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IDoctorRepository DoctorRepository { get; }

    }
}
