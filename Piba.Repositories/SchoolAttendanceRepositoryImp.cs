using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class SchoolAttendanceRepositoryImp : SchoolAttendanceRepository
    {
        private PibaDbContext _dbContext;

        public SchoolAttendanceRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(SchoolAttendance schoolAttendance)
        {
            _dbContext.Entry(schoolAttendance).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Dictionary<DateOnly, List<AttendanceReportDto>>> GetAttendancesReportAsync(List<DateOnly> list, TimeSpan maxTime)
        {
            var result = await _dbContext.Set<SchoolAttendance>()
                .Include(s => s.Member)
                .Where(s => s.Member.Status != MemberStatus.Removed
                    && s.IsPresent 
                    && s.CreatedDate.Value.TimeOfDay <= maxTime
                ).ToListAsync();

            return list.ToDictionary(l => l,
                l => result.Where(r => l == DateOnly.FromDateTime(r.CreatedDate.Value))
                .Select(r => new AttendanceReportDto
                {
                    Name = r.Member.Name,
                    Time = TimeOnly.FromTimeSpan(r.CreatedDate.Value.TimeOfDay),
                })
                .ToList());
            
        }

        public async Task<int> GetByDatesAsync(MemberAttendancesByDatesFilter filter)
        {
            return await _dbContext.Set<SchoolAttendance>()
                .Where(a =>
                    a.MemberId == filter.MemberId
                    && filter.Dates.Contains(a.CreatedDate.Value.Date)
                    && a.CreatedDate.Value.TimeOfDay >= filter.MinValidTime
                    && a.CreatedDate.Value.TimeOfDay <= filter.MaxValidTime)
                .Select(a => a.CreatedDate)
                .Distinct()
                .CountAsync();
        }

        public async Task<List<SchoolAttendance>> GetLastMonthExcusesAsync()
        {
            var aMonthAgo = DateTime.UtcNow.AddMonths(-1);
            return await _dbContext.Set<SchoolAttendance>()
                .Include(a => a.Member)
                .Where(a =>
                    a.CreatedDate.Value.Month == aMonthAgo.Month
                    && a.CreatedDate.Value.Year == aMonthAgo.Year
                    && a.IsPresent == false
                    )
                .ToListAsync();
        }
    }
}
