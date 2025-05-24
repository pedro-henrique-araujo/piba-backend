using Moq;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;
using Piba.Repositories.Interfaces;

namespace Piba.Services.Tests
{
    public class LinkServiceImpTests
    {
        private readonly Mock<LinkRepository> _linkRepository;
        private LinkServiceImp _linkService;
        public LinkServiceImpTests()
        {
            _linkRepository = new Mock<LinkRepository>();
            _linkService = new LinkServiceImp(_linkRepository.Object);
        }

        [Fact]
        public async Task CreateLinksAsync_WhenCalled_PerformCorrectly()
        {
            var id = Guid.NewGuid();
            var links = new List<LinkDto>
            {
                new()
                {
                    Url = "example.com",
                    Source = LinkSource.Youtube
                },
                new()
                {
                    Url = "example2.com",
                    Source = LinkSource.Youtube
                },
            };

            await _linkService.CreateLinksAsync(id, links);

            _linkRepository.Verify(x => x.CreateRangeAsync(It.Is<List<Link>>(l =>
                    l.Count == 2
                    && l[0].SongId == id
                    && l[0].Url == "example.com"
                    && l[0].Source == LinkSource.Youtube
                    && l[1].SongId == id
                    && l[1].Url == "example2.com"
                    && l[1].Source == LinkSource.Youtube
                )), Times.Once);
        }

        [Fact]
        public async Task UpdateLinksAsync_WhenCalled_PerformProperly()
        {

            var linksInDb = new List<Link>
            {
                new()
                {
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid()
                },
            };

            var songId = Guid.NewGuid();

            _linkRepository
                .Setup(x => x.GetBySongIdAsync(songId))
                .ReturnsAsync(linksInDb);

            await _linkService.UpdateLinksAsync(songId, new()
            {
                new()
                {
                    Url = "example.com",
                    Source = LinkSource.Youtube
                },
                new()
                {
                    Url = "example2.com",
                    Source = LinkSource.Youtube
                },
                new()
                {
                    Id = linksInDb[0].Id,
                    Url = "example3.com",
                    Source = LinkSource.Youtube
                },
                new()
                {
                    Id = linksInDb[1].Id,
                    Url = "example4.com",
                    Source = LinkSource.Youtube
                },
            });

            _linkRepository.Verify(x => x.CreateRangeAsync(It.Is<List<Link>>(l =>
                    l.Count == 2
                    && l[0].Id == Guid.Empty
                    && l[0].SongId == songId
                    && l[0].Url == "example.com"
                    && l[0].Source == LinkSource.Youtube
                    && l[1].Id == Guid.Empty
                    && l[1].SongId == songId
                    && l[1].Url == "example2.com"
                    && l[1].Source == LinkSource.Youtube
                )), Times.Once);

            _linkRepository.Verify(x => x.UpdateRangeAsync(It.Is<List<Link>>(l =>
                    l.Count == 2
                    && l[0].Id == linksInDb[0].Id
                    && l[0].SongId == songId
                    && l[0].Url == "example3.com"
                    && l[0].Source == LinkSource.Youtube
                    && l[1].Id == linksInDb[1].Id
                    && l[1].SongId == songId
                    && l[1].Url == "example4.com"
                    && l[1].Source == LinkSource.Youtube
                )), Times.Once);

            _linkRepository.Verify(x => x.DeleteRangeAsync(It.Is<List<Link>>(l =>
                   l.Count == 2
                   && l[0].Id == linksInDb[2].Id
                   && l[1].Id == linksInDb[3].Id
               )), Times.Once);

        }
    }
}
