using Piba.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Piba.Services
{
    public class SmtpClientWrapperImp : SmtpClientWrapper
    {
        private readonly EnvironmentVariables _environmentVariables;

        public SmtpClientWrapperImp(EnvironmentVariables environmentVariables)
        {
            _environmentVariables = environmentVariables;
        }

        public void Send(MailMessage mailMessage)
        {
            var client = new SmtpClient
            {
                Host = _environmentVariables.EmailHost,
                Port = 587,
                Credentials = new NetworkCredential(_environmentVariables.FromEmail, _environmentVariables.FromPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            client.Send(mailMessage);
        }
    }
}
