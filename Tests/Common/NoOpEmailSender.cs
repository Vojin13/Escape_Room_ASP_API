using Application.Emails;

namespace Tests.Common
{
    public class NoOpEmailSender : IEmailSender
    {
        public void SendEmail(string recipient, string subject, string htmlContent)
        {
        }
    }
}
