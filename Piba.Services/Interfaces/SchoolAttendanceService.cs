using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface SchoolAttendanceService
    {
        Task CreateAsync(SchoolAttendance schoolAttendance);
    }
}
