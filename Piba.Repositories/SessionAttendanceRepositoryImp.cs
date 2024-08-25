using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class SessionAttendanceRepositoryImp : SessionAttendanceRepository
    {
        private readonly PibaDbContext _dbContext;

        public SessionAttendanceRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(SessionAttendance attendance)
        {
            _dbContext.Entry(attendance).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var record = await _dbContext.Set<SessionAttendance>().FindAsync(id);
            _dbContext.Entry(record).State = EntityState.Deleted;
        }

        public async Task<SessionAttendance> GetByIdAsync(Guid id)
        {
            var record = await _dbContext.Set<SessionAttendance>()
                .Include(s => s.SessionAttendanceItems)
                    .ThenInclude(i => i.Member)
                .FirstOrDefaultAsync(s => s.Id == id);
            return record;
        }

        public async Task<int> GetTotalAsync()
        {
            var total = await _dbContext.Set<SessionAttendance>().CountAsync();
            return total;
        }

        public async Task<List<SessionAttendance>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var records = await _dbContext.Set<SessionAttendance>()
                    .Skip(paginationQueryParameters.Skip)
                    .Take(paginationQueryParameters.Take)
                .ToListAsync();

            return records;
        }
    }
}
