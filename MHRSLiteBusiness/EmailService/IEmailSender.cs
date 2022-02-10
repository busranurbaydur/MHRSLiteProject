using MHRSLiteEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusiness.EmailService
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}
