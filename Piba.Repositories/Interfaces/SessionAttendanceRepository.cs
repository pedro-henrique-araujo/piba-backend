using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface SessionAttendanceRepository
    {
        Task CreateAsync(SessionAttendance attendance);
        Task DeleteAsync(Guid id);
        Task<SessionAttendance> GetByIdAsync(Guid id);
        Task<int> GetTotalAsync();
        Task<List<SessionAttendance>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
        Task UpdateAsync(SessionAttendance attendance);
    }
}
