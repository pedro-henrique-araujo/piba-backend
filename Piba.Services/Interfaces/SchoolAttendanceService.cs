using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface SchoolAttendanceService
    {
        Task CreateAsync(SchoolAttendance schoolAttendance);
        Task<bool> MemberIsPresentAtLeastOnceOnLastThreeSaturdaysAsync(Guid memberId);
        Task<bool> MemberMissedAnyOfLastThreeClassesAsync(Guid memberId);
    }
}
