using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class LinkRepositoryImp : LinkRepository
    {
        private readonly PibaDbContext _pibaDbContext;
        public LinkRepositoryImp(PibaDbContext pibaDbContext)
        {
            _pibaDbContext = pibaDbContext;
        }

        public async Task CreateRangeAsync(List<Link> links)
        {
            var set = _pibaDbContext.Set<Link>();
            await set.AddRangeAsync(links);
            await _pibaDbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<Link> links)
        {
            var set = _pibaDbContext.Set<Link>();
            set.RemoveRange(links);
            await _pibaDbContext.SaveChangesAsync();
        }

        public async Task<List<Link>> GetBySongIdAsync(Guid songId)
        {
            var links = await _pibaDbContext.Set<Link>()
                .Where(l => l.SongId == songId)
                .AsNoTracking()
                .ToListAsync();

            return links;
        }

        public async Task UpdateRangeAsync(List<Link> links)
        {
            var set = _pibaDbContext.Set<Link>();
            set.UpdateRange(links);
            await _pibaDbContext.SaveChangesAsync();
        }
    }
}
