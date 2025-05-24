using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class SongRepositoryImp : SongRepository
    {
        private readonly PibaDbContext _dbContext;

        public SongRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Song song)
        {
            _dbContext.Entry(song).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var record = await _dbContext.Set<Song>().FindAsync(id);
            _dbContext.Entry(record).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Song> GetByIdAsync(Guid id)
        {
            var record = await _dbContext.Set<Song>()
                .FirstOrDefaultAsync(s => s.Id == id);
            return record;
        }

        public async Task<int> GetTotalAsync()
        {
            var total = await _dbContext.Set<Song>().CountAsync();
            return total;
        }

        public async Task<List<Song>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var records = await _dbContext.Set<Song>()
                    .Skip(paginationQueryParameters.Skip)
                    .Take(paginationQueryParameters.Take)                    
                .ToListAsync();

            return records;
        }

        public async Task UpdateAsync(Song song)
        {
            _dbContext.Entry(song).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(song).State = EntityState.Detached;
        }
    }
}
