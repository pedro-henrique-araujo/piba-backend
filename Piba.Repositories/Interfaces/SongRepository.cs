using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface SongRepository
    {
        Task CreateAsync(Song song);
        Task DeleteAsync(Guid id);
        Task<Song> GetByIdAsync(Guid id);
        Task<int> GetTotalAsync();
        Task<List<Song>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
        Task UpdateAsync(Song song);
    }
}
