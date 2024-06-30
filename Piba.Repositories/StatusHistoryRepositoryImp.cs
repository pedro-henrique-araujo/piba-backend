using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace Piba.Repositories
{
    public class StatusHistoryRepositoryImp : StatusHistoryRepository
    {
        private readonly PibaDbContext _pibaDbContext;

        public StatusHistoryRepositoryImp(PibaDbContext pibaDbContext)
        {
            _pibaDbContext = pibaDbContext;
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

        public async Task<bool> IsHistoryOfLastMonthSentAsync()
        {
            var aMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var historySet = _pibaDbContext.Set<StatusHistory>();
            var isSent = await historySet
                .Where(h => h.Year == aMonthAgo.Year && h.Month == aMonthAgo.Month)
                .Select(h => h.IsSent)
                .FirstOrDefaultAsync();
            return isSent;
        }

        public async Task MarkLastMonthHistoryAsSentAsync()
        {
            var aMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var historySet = _pibaDbContext.Set<StatusHistory>();
            var history = await historySet
                .Where(h => h.Year == aMonthAgo.Year && h.Month == aMonthAgo.Month)
                .FirstOrDefaultAsync();
            if (history is null) return;
            history.IsSent = true;
            await _pibaDbContext.SaveChangesAsync();
        }
    }
}
