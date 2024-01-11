using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class SchoolAttendanceServiceImp : SchoolAttendanceService
    {
        private SchoolAttendanceRepository _schoolAttendanceRepository;

        public SchoolAttendanceServiceImp(SchoolAttendanceRepository schoolAttendanceRepository)
        {
            _schoolAttendanceRepository = schoolAttendanceRepository;
        }

        public async Task CreateAsync(SchoolAttendance schoolAttendance)
        {
            schoolAttendance.Create();
            await _schoolAttendanceRepository.CreateAsync(schoolAttendance);
        }
    }
}
