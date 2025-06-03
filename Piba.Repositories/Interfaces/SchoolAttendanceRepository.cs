using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface SchoolAttendanceRepository
    {
        Task CreateAsync(SchoolAttendance schoolAttendance);
        Task<Dictionary<DateOnly, List<AttendanceReportDto>>> GetAttendancesReportAsync(List<DateOnly> list, TimeSpan maxTime);
        Task<int> GetByDatesAsync(MemberAttendancesByDatesFilter filter);
        Task<List<SchoolAttendance>> GetLastMonthExcusesAsync();
    }
}