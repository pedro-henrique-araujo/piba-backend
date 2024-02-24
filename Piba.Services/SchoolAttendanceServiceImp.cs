using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class SchoolAttendanceServiceImp : SchoolAttendanceService
    {
        private readonly SchoolAttendanceRepository _schoolAttendanceRepository;
        private readonly SaturdayWithoutClassService _saturdayWithoutClassService;

        public SchoolAttendanceServiceImp(SchoolAttendanceRepository schoolAttendanceRepository, SaturdayWithoutClassService saturdayWithoutClassService)
        {
            _schoolAttendanceRepository = schoolAttendanceRepository;
            _saturdayWithoutClassService = saturdayWithoutClassService;
        }

        public async Task CreateAsync(SchoolAttendance schoolAttendance)
        {
            schoolAttendance.Create();
            await _schoolAttendanceRepository.CreateAsync(schoolAttendance);
        }

        public async Task<bool> MemberIsPresentAtLeastOnceOnLastThreeSaturdaysAsync(Guid memberId)
        {
            var count = await _schoolAttendanceRepository.GetByDatesAsync(
                new()
                {
                    MemberId = memberId,
                    Dates = await GetLastThreeClassesDatesAsync()
                });
            return count > 0;
        }

        public async Task<bool> MemberMissedAnyOfLastThreeClassesAsync(Guid memberId)
        {
            var count = await _schoolAttendanceRepository.GetByDatesAsync(
                new()
                {
                    MemberId = memberId,
                    Dates = await GetLastThreeClassesDatesAsync()
                });
            return count < 3;
        }

        private async Task<List<DateTime>> GetLastThreeClassesDatesAsync()
        {
            var output = new List<DateTime>();
            var today = DateTime.Today;
            var selectedSaturday = today.AddDays(1 - (double)today.DayOfWeek);
            do
            {
                if (await _saturdayWithoutClassService.AnyWithDateAsync(selectedSaturday))
                {
                    output.Add(selectedSaturday);
                }
                selectedSaturday = selectedSaturday.AddDays(-7);

            } while (output.Count() < 3);
            return output;
        }
    }
}
