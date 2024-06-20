using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface StatusHistoryRepository
    {
        Task CreateAsync(StatusHistory statusHistory);
        Task<bool> HistoryForLastMonthExistsAsync();
        Task<bool> IsHistoryOfLastMonthSent();
        Task MarkLastMonthHistoryAsSentAsync();
    }
}
