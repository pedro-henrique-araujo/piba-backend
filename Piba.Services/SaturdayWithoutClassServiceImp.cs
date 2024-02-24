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
        public async Task<bool> AnyWithDateAsync(DateTime date)
        {
            return await _saturdayWithoutClassRepository.AnyWithDateAsync(date);
        }
    }
}
