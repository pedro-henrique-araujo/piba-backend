using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class SchoolAttendanceRepositoryImp : SchoolAttendanceRepository
    {
        private PibaDbContext _dbContext;

        public SchoolAttendanceRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(SchoolAttendance schoolAttendance)
        {
            _dbContext.Entry(schoolAttendance).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }
    }
}
