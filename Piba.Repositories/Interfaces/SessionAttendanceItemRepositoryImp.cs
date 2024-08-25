using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public class SessionAttendanceItemRepositoryImp : SessionAttendanceItemRepository
    {
        private readonly PibaDbContext _dbContext;

        public SessionAttendanceItemRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateRangeAsync(IEnumerable<SessionAttendanceItem> items)
        {
            await _dbContext.Set<SessionAttendanceItem>().AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<SessionAttendanceItem>> GetBySessionAttendanceIdAsync(Guid sessionAttendanceId)
        {
            var records = await _dbContext.Set<SessionAttendanceItem>()
                .Where(i => i.SessionAttendanceId == sessionAttendanceId)
                .ToListAsync();

            return records;
        }

        public async Task UpdateRangeAsync(IEnumerable<SessionAttendanceItem> itemsToUpdate)
        {
            _dbContext.Set<SessionAttendanceItem>().UpdateRange(itemsToUpdate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
