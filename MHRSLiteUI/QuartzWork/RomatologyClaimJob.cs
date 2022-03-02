using MHRSLiteBusiness.Contracts;
using MHRSLiteEntity.Enums;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.QuartzWork
{
    public class RomatologyClaimJob:IJob
    {

        private readonly ILogger<RomatologyClaimJob> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public RomatologyClaimJob(ILogger<RomatologyClaimJob> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                //son bir aydaki dahiliyedeki iptal olan hariç tüm randevuları getir..

                var date = DateTime.Now.AddMonths(-1);
                var appointment = _unitOfWork.AppointmentRepository.GetAppointmentsIM(date).OrderByDescending(x=>x.AppointmentDate).ToList();

                foreach (var item in appointment)
                {
                    //user a ait dahiliye romatoloji claimi yoksa eklenmeli.
                    //////////////
                    /////////////
                    /////////////

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
