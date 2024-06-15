using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class StatusHistoryRepositoryImp : StatusHistoryRepository
    {
        private readonly PibaDbContext _pibaDbContext;

        public StatusHistoryRepositoryImp(PibaDbContext pibaDbContext)
        {
            _pibaDbContext = pibaDbContext;
        }

        public async Task CreateAsync(IEnumerable<StatusHistoryItem> histories)
        {
            var historySet = _pibaDbContext.Set<StatusHistoryItem>();
            historySet.AddRange(histories);
            await _pibaDbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(StatusHistory statusHistory)
        {
            _pibaDbContext.Entry(statusHistory).State = EntityState.Added;
            await _pibaDbContext.SaveChangesAsync();
        }

        public async Task<bool> HistoryForLastMonthExistsAsync()
        {
            var historySet = _pibaDbContext.Set<StatusHistory>();
            var aMonthAgo = DateTime.UtcNow.AddMonths(-1);
            return await historySet.AnyAsync(h => h.Year == aMonthAgo.Year && h.Month == aMonthAgo.Month);
        }
    }
}
