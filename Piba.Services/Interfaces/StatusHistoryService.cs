
namespace Piba.Services.Interfaces
{
    public interface StatusHistoryService
    {
        Task CreateForLastMonthIfItDoesNotExistAsync();
        Task SendStatusHistoryEmailToReceiversAsync();
    }
}
