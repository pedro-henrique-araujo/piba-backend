using Mapster;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class LinkServiceImp : LinkService
    {
        private readonly LinkRepository _linkRepository;

        public LinkServiceImp(LinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        public async Task CreateLinksAsync(Guid songId, List<LinkDto> links)
        {
            var linksToSave = links.Adapt<List<Link>>();
            foreach (var item in linksToSave)
            {
                item.SongId = songId;
            }
            await _linkRepository.CreateRangeAsync(linksToSave);
        }

        public async Task UpdateLinksAsync(Guid songId, List<LinkDto> links)
        {
            var linksInDb = await _linkRepository.GetBySongIdAsync(songId);

            var linksToCreate = links.Where(l => !linksInDb.Any(ld => ld.Id == l.Id)).Adapt<List<Link>>();
            foreach (var item in linksToCreate)
            {
                item.SongId = songId;
            }
            await _linkRepository.CreateRangeAsync(linksToCreate);

            var linksToUpdate = links.Where(l => linksInDb.Any(ld => ld.Id == l.Id)).Adapt<List<Link>>();

            foreach (var item in linksToUpdate)
            {
                item.SongId = songId;
            }
            await _linkRepository.UpdateRangeAsync(linksToUpdate);


            var linksToDelete = linksInDb.Where(l => !links.Any(ld => ld.Id == l.Id)).ToList();
            await _linkRepository.DeleteRangeAsync(linksToDelete);

        }
    }
}
