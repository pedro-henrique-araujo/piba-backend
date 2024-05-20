using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface MemberStatusHistoryRepository
    {
        Task CreateAsync(IEnumerable<MemberStatusHistory> histories);
        Task<bool> HistoryForLastMonthExistsAsync();
    }
}
