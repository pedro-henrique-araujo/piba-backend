using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface SessionAttendanceItemRepository
    {
        Task CreateRangeAsync(IEnumerable<SessionAttendanceItem> items);
        Task<List<SessionAttendanceItem>> GetBySessionAttendanceIdAsync(Guid sessionAttendanceId);
        Task UpdateRangeAsync(IEnumerable<SessionAttendanceItem> itemsToUpdate);
    }
}
