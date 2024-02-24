using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface SchoolAttendanceRepository
    {
        Task CreateAsync(SchoolAttendance schoolAttendance);
        Task<int> GetByDatesAsync(MemberClassesByDatesFilter filter);
    }
}