using Application.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Emails
{
    public class SmtpEmailSender : IEmailSender
    {
        private string _fromEmail;
        private string _appPassword;

        public SmtpEmailSender(string fromEmail, string appPassword)
        {
            this._fromEmail = fromEmail;
            this._appPassword = appPassword;
        }

        public void SendEmail(string recipient, string subject, string htmlContent)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_fromEmail, "ASP ICT 2026"),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };

            message.To.Add(recipient);

            using SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_fromEmail, _appPassword)
            };

            client.Send(message);
        }
    }
}