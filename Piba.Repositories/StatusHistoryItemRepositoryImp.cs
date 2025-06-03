using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
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

        public async Task<List<AttendanceReportDto>> GetAttendancesReportAsync(DateOnly date)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StatusHistoryReportDto>> GetLastHistoryAsync(ValidTimeFilter filter)
        {
            var aMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var set = _pibaDbContext.Set<StatusHistoryItem>();
            return await set.Where(i =>
                        i.StatusHistory.Month == aMonthAgo.Month
                        && i.StatusHistory.Year == aMonthAgo.Year
                    )
                .Select(i => new StatusHistoryReportDto
                    {
                        Name = i.Member.Name,
                        Status = i.Status,
                        Count = i.Member.SchoolAttendances.Count(a => 
                            a.CreatedDate.Value.Month == aMonthAgo.Month 
                            && a.CreatedDate.Value.Year == aMonthAgo.Year
                             && (a.IsPresent == false
                                   || a.CreatedDate.Value.TimeOfDay >= filter.MinValidTime
                                   && a.CreatedDate.Value.TimeOfDay <= filter.MaxValidTime
                               ))
                    })
                .OrderBy(i => i.Name)
                .ToListAsync();
        }
    }
}
