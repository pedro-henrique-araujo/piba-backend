using Piba.Data.Dto;

namespace Piba.Services.Interfaces
{
    public interface EmailService
    {
        void SendEmailToDeveloper(SendEmailDto dto);
    }
}