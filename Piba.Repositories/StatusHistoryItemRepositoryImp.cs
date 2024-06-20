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

        public async Task<List<StatusHistoryReportDto>> GetLastHistoryAsync(ValidTimeFilter filter)
        {
            var aMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var lastMonthSchoolAttendance = _pibaDbContext.Set<SchoolAttendance>()
                .Where(a => a.CreatedDate.Value.Month == aMonthAgo.Month
                    && a.CreatedDate.Value.Year == aMonthAgo.Year
                    && (a.IsPresent == false || 
                        a.CreatedDate.Value.TimeOfDay >= filter.MinValidTime
                    && a.CreatedDate.Value.TimeOfDay <= filter.MaxValidTime));

            var set = _pibaDbContext.Set<StatusHistoryItem>();
            return await set
                .Where(i => i.StatusHistory.Month == aMonthAgo.Month
                    && i.StatusHistory.Year == aMonthAgo.Year)
                .Include(i => i.Member)
                .Join(
                    _pibaDbContext.Set<SchoolAttendance>(),
                    i => i.MemberId,
                    a => a.MemberId,
                    (item, atendance) => new
                    {
                        item.Member,
                        Attendance = atendance

                    })
                .GroupBy(i => i.Member)
                .Select(g => new StatusHistoryReportDto
                {
                    Name = g.Key.Name,
                    Status = g.Key.Status,
                    Count = g.Count(),
                }).ToListAsync();
        }
    }
}
