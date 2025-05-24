using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface SongService
    {
        Task CreateAsync(SongDto song);
        Task DeleteAsync(Guid id);
        Task<SongDto> GetByIdAsync(Guid id);
        Task<RecordsPage<Song>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
        Task UpdateAsync(SongDto songDto);
    }
}