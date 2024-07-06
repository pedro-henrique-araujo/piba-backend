using Piba.Data.Dto;

namespace Piba.Services.Interfaces
{
    public interface EmailService
    {
        void SendEmailToDeveloper(SendEmailDto dto);
        void SendEmailToDeveloper(SendEmailDto dto, byte[] attachment);
    }
}