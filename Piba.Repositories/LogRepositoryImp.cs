using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class LogRepositoryImp : LogRepository
    {
        private readonly PibaDbContext _dbContext;

        public LogRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogMessageAsync(string message)
        {
            var log = new Log(message);
            _dbContext.Entry(log).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }
    }
}
