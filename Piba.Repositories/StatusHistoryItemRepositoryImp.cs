using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class StatusHistoryItemRepositoryImp : StatusHistoryItemRepository
    {
        private readonly PibaDbContext _pibaDbContext;

        public StatusHistoryItemRepositoryImp(PibaDbContext pibaDbContext)
        {
            _pibaDbContext = pibaDbContext;
        }

        public async Task CreateAsync(IEnumerable<StatusHistoryItem> items)
        {
            var historyItem = _pibaDbContext.Set<StatusHistoryItem>();

            await historyItem.AddRangeAsync(items);

            await _pibaDbContext.SaveChangesAsync();
        }
    }
}
