using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;
using Mapster;

namespace Piba.Services
{
    public class SongServiceImp : SongService
    {
        private readonly SongRepository _songRepository;
        private readonly LinkService _linkService;

        public SongServiceImp(SongRepository songRepository, LinkService linkService)
        {
            _songRepository = songRepository;
            _linkService = linkService;
        }

        public async Task<RecordsPage<Song>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var page = new RecordsPage<Song>();
            page.Records = await _songRepository.PaginateAsync(paginationQueryParameters);
            page.Total = await _songRepository.GetTotalAsync();
            return page;
        }

        public async Task<SongDto> GetByIdAsync(Guid id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            return song.Adapt<SongDto>();
        }

        public async Task CreateAsync(SongDto song)
        {
            var songCreated = await _songRepository.CreateAsync(song.Adapt<Song>());
            await _linkService.CreateLinksAsync(songCreated.Id, song.Links);
        }

        public async Task UpdateAsync(SongDto songDto)
        {
            await _songRepository.UpdateAsync(songDto.Adapt<Song>());
            await _linkService.UpdateLinksAsync(songDto.Id.Value, songDto.Links);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _songRepository.DeleteAsync(id);
        }
    }
}
