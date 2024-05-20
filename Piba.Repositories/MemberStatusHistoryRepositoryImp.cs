using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class MemberStatusHistoryRepositoryImp : MemberStatusHistoryRepository
    {
        private readonly PibaDbContext _pibaDbContext;

        public MemberStatusHistoryRepositoryImp(PibaDbContext pibaDbContext)
        {
            _pibaDbContext = pibaDbContext;
        }

        public async Task CreateAsync(IEnumerable<MemberStatusHistory> histories)
        {
            var historySet = _pibaDbContext.Set<MemberStatusHistory>();
            historySet.AddRange(histories);
            await _pibaDbContext.SaveChangesAsync();
        }

        public async Task<bool> HistoryForLastMonthExistsAsync()
        {
            var historySet = _pibaDbContext.Set<MemberStatusHistory>();
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            return await historySet.AnyAsync(h => 
                h.HistoryMonth.Month == lastMonth.Month
                && h.HistoryMonth.Year == lastMonth.Year);
        }
    }
}
