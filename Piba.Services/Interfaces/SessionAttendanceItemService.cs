using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface SessionAttendanceItemService
    {
        Task CreateRangeAsync(Guid sessionAttendanceId, List<SessionAttendanceItemDto> items);
        Task UpdateRangeAsync(Guid sessionAttendanceId, List<SessionAttendanceItemUpdateDto> items);
    }
}