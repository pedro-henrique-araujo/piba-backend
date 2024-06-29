using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class SaturdayWithoutClassServiceImp : SaturdayWithoutClassService
    {
        private readonly SaturdayWithoutClassRepository _saturdayWithoutClassRepository;
        public SaturdayWithoutClassServiceImp(SaturdayWithoutClassRepository saturdayWithoutClassRepository)
        {
            _saturdayWithoutClassRepository = saturdayWithoutClassRepository;
        }

        public async Task<List<DateTime>> GetLastThreeClassesDatesAsync()
        {
            var output = new List<DateTime>();
            var today = DateTime.Today;
            var selectedSaturday = today.AddDays(-1 - (double)today.DayOfWeek);
            do
            {
                if (await _saturdayWithoutClassRepository.DateWouldHaveClassAsync(selectedSaturday))
                {
                    output.Add(selectedSaturday);
                }
                selectedSaturday = selectedSaturday.AddDays(-7);

            } while (output.Count() < 3);
            return output;
        }
    }
}
