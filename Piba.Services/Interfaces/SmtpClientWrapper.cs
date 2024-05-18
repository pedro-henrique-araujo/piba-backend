using System.Net.Mail;

namespace Piba.Services.Interfaces
{
    public interface SmtpClientWrapper
    {
        void Send(MailMessage mailMessage);
    }
}