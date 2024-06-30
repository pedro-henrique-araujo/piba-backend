using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class SaturdayWithoutClassRepositoryImp : SaturdayWithoutClassRepository
    {
        private readonly PibaDbContext _dbContext;

        public SaturdayWithoutClassRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DateWouldHaveClassAsync(DateTime date)
        {
            return await _dbContext.Set<SaturdayWithoutClass>()
                .AnyAsync(s => s.DateTime.Date == date.Date) == false;
        }
    }
}
