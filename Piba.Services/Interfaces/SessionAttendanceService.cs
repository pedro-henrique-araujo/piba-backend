using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface SessionAttendanceService
    {
        Task CreateAsync(SessionAttendanceDto sessionAttendanceDto);
        Task DeleteAsync(Guid id);
        Task<SessionAttendanceOutputDto> GetByIdAsync(Guid id);
        Task<RecordsPage<SessionAttendance>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
        Task UpdateAsync(SessionAttendanceUpdateDto sessionAttendanceUpdateDto);
    }
}
