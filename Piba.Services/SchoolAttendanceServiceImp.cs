using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class SchoolAttendanceServiceImp : SchoolAttendanceService
    {
        private readonly EnvironmentVariables _envioronmentVariables;
        private readonly SchoolAttendanceRepository _schoolAttendanceRepository;
        private readonly SaturdayWithoutClassService _saturdayWithoutClassService;

        public SchoolAttendanceServiceImp(
            EnvironmentVariables environmentVariables,
            SchoolAttendanceRepository schoolAttendanceRepository,
            SaturdayWithoutClassService saturdayWithoutClassService)
        {
            _envioronmentVariables = environmentVariables;
            _schoolAttendanceRepository = schoolAttendanceRepository;
            _saturdayWithoutClassService = saturdayWithoutClassService;
        }

        public async Task CreateAsync(SchoolAttendance schoolAttendance)
        {
            schoolAttendance.Create();
            await _schoolAttendanceRepository.CreateAsync(schoolAttendance);
        }

        public async Task<bool> MemberIsPresentAtLeastOnceOnLastThreeClassesAsync(Guid memberId)
        {
            var count = await _schoolAttendanceRepository.GetByDatesAsync(
                new()
                {
                    MemberId = memberId,
                    Dates = await _saturdayWithoutClassService.GetLastThreeClassesDatesAsync(),
                    MinValidTime = _envioronmentVariables.MinValidTime,
                    MaxValidTime = _envioronmentVariables.MaxValidTime
                });
            return count > 0;
        }

        public async Task<bool> MemberMissedAnyOfLastThreeClassesAsync(Guid memberId)
        {
            var count = await _schoolAttendanceRepository.GetByDatesAsync(
                new()
                {
                    MemberId = memberId,
                    Dates = await _saturdayWithoutClassService.GetLastThreeClassesDatesAsync(),
                    MinValidTime = _envioronmentVariables.MinValidTime,
                    MaxValidTime = _envioronmentVariables.MaxValidTime
                });
            return count < 3;
        }
    }
}
