using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;
using Mapster;
using Piba.Data;
using Microsoft.EntityFrameworkCore;

namespace Piba.Services
{
    public class SongServiceImp : SongService
    {
        private readonly SongRepository _songRepository;
        private readonly LinkService _linkService;
        PibaDbContext _db;

        public SongServiceImp(SongRepository songRepository, LinkService linkService, PibaDbContext db)
        {
            _songRepository = songRepository;
            _linkService = linkService;
            _db = db;
        }

        public async Task<RecordsPage<Song>> PaginateAsync(BrowseQueryParameters browseQueryParameters)
        {
            var page = new RecordsPage<Song>();

            var query = _db.Songs.AsQueryable();

            if (browseQueryParameters.Search is not null)
            {
                query = query.Where(s => EF.Functions.Like(s.Name, $"%{browseQueryParameters.Search}%"));
            }

            page.Total = await query.CountAsync();

            query = query.Skip(browseQueryParameters.Skip).Take(browseQueryParameters.Take);
            page.Records = await query.ToListAsync();
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
