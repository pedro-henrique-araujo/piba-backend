using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
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

        public async Task<int> GetByDatesAsync(MemberClassesByDatesFilter filter)
        {
            return await _dbContext.Set<SchoolAttendance>()
                .Where(a => a.MemberId == filter.MemberId && filter.Dates.Contains(a.CreatedDate.GetValueOrDefault().Date))
                .CountAsync();
        }
    }
}
