using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class SongServiceImp : SongService
    {
        private readonly SongRepository _songRepository;

        public SongServiceImp(SongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public async Task<RecordsPage<Song>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var page = new RecordsPage<Song>();
            page.Records = await _songRepository.PaginateAsync(paginationQueryParameters);
            page.Total = await _songRepository.GetTotalAsync();
            return page;
        }

        public async Task<Song> GetByIdAsync(Guid id)
        {
            return await _songRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Song song)
        {
            await _songRepository.CreateAsync(song);
        }

        public async Task UpdateAsync(Song song)
        {
            await _songRepository.UpdateAsync(song);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _songRepository.DeleteAsync(id);
        }
    }
}
