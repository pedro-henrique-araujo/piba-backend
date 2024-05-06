using Piba.Data.Dto;
using Piba.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Piba.Services
{
    public class EmailServiceImp : EmailService
    {
        private readonly string _developerEmail;
        private readonly string _from;
        private readonly string _fromUserName;
        private readonly string _fromPassword;
        private readonly string _emailHost;

        public EmailServiceImp()
        {
            _emailHost = Environment.GetEnvironmentVariable("SmtpHost");
            _developerEmail = Environment.GetEnvironmentVariable("DeveloperEmail");
            _from = Environment.GetEnvironmentVariable("FromEmail");
            _fromUserName = Environment.GetEnvironmentVariable("FromEmailUserName");
            _fromPassword = Environment.GetEnvironmentVariable("FromEmailPassword");
        }

        public void SendEmailToDeveloper(SendEmailDto dto)
        {

            var email = new MailMessage(_from, _developerEmail)
            {
                Subject = dto.Subject,
                Body = dto.Body
            };

            var smtpClient = CreateSmtpClientToDeveloper();

            smtpClient.Send(email);
        }

        private SmtpClient CreateSmtpClientToDeveloper()
        {
            return new SmtpClient
            {
                Host = _emailHost,
                Port = 587,
                Credentials = new NetworkCredential(_fromUserName, _fromPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
        }
    }
}
