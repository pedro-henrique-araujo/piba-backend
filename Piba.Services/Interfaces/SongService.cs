using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface SongService
    {
        Task CreateAsync(Song song);
        Task DeleteAsync(Guid id);
        Task<Song> GetByIdAsync(Guid id);
        Task<RecordsPage<Song>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
        Task UpdateAsync(Song song);
    }
}