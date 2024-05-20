
namespace Piba.Services.Interfaces
{
    public interface MemberStatusHistoryService
    {
        Task CreateActivityHistoryForLastMonthIfItDoesNotExistAsync();
        Task SendMemberStatusHistoryEmailToReceiversAsync();
    }
}
