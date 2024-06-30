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
        private readonly SchoolAttendanceRepository _schoolAttendanceRepository;

        public StatusHistoryItemRepositoryImp(PibaDbContext pibaDbContext, SchoolAttendanceRepository schoolAttendanceRepository)
        {
            _pibaDbContext = pibaDbContext;
            _schoolAttendanceRepository = schoolAttendanceRepository;
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
            var lastMonthSchoolAttendance = _schoolAttendanceRepository
                .GetLastMonthsSchoolAttendanceQueryable(filter);

            var set = _pibaDbContext.Set<StatusHistoryItem>();
            return await set
                .Where(i =>
                        i.StatusHistory.Month == aMonthAgo.Month
                        && i.StatusHistory.Year == aMonthAgo.Year
                    )
                .Join(
                    lastMonthSchoolAttendance,
                    i => i.MemberId,
                    a => a.MemberId,
                    (item, atendance) => new
                    {
                        item.Member,
                        item.Status,
                        Attendance = atendance

                    })
                .GroupBy(i => i.Member)
                .Select(g => new StatusHistoryReportDto
                {
                    Name = g.Key.Name,
                    Status = g.First().Status,
                    Count = g.Count(),
                })
                .OrderBy(i => i.Name)
                .ToListAsync();
        }
    }
}
