using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface StatusHistoryItemRepository
    {
        Task CreateAsync(IEnumerable<StatusHistoryItem> items);
        Task<List<StatusHistoryReportDto>> GetLastHistoryAsync(ValidTimeFilter filter);
    }
}
