using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface SchoolAttendanceRepository
    {
        Task CreateAsync(SchoolAttendance schoolAttendance);
    }
}