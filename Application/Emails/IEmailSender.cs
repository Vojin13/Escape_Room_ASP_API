using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Emails
{
    public interface IEmailSender
    {
        void SendEmail(string recipient, string subject, string htmlContent);
    }
}
