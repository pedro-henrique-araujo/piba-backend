using Moq;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class SongServiceImpTests
    {
        private readonly Mock<SongRepository> _songRepositoryMock;
        private readonly Mock<LinkService> _linkServiceMock;
        private readonly SongServiceImp _songServiceImp;

        public SongServiceImpTests()
        {
            _songRepositoryMock = new Mock<SongRepository>();
            _linkServiceMock = new Mock<LinkService>();
            _songServiceImp = new SongServiceImp(_songRepositoryMock.Object, _linkServiceMock.Object);
        }

        [Fact]
        public async Task PaginateAsync_WhenCalled_ReturnExpectedResult()
        {
            var pagination = new PaginationQueryParameters();
            var records = new List<Song>();
            var total = 15;

            _songRepositoryMock.Setup(r => r.PaginateAsync(pagination))
                .ReturnsAsync(records);

            _songRepositoryMock.Setup(r => r.GetTotalAsync())
                .ReturnsAsync(total);

            var result = await _songServiceImp.PaginateAsync(pagination);

            Assert.Equal(records, result.Records);
            Assert.Equal(total, result.Total);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalled_AssertCorrectResult()
        {
            var song = new Song { Name = "test" };
            var id = Guid.NewGuid();

            _songRepositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(song);

            var result = await _songServiceImp.GetByIdAsync(id);

            Assert.Equal(song.Name, result.Name);

        }

        [Fact]
        public async Task CreateAsync_WhenCalled_CreateCorrectly()
        {
            var song = new SongDto {   Name = "test", Links = new() };
            var songCreated = new Song { Id = Guid.NewGuid()};
            _songRepositoryMock.Setup(r => r.CreateAsync(It.Is<Song>(s => s.Name == "test")))
                .ReturnsAsync(songCreated);

            await _songServiceImp.CreateAsync(song);

            _linkServiceMock.Verify(l => l.CreateLinksAsync(songCreated.Id, song.Links));

        }

        [Fact]
        public async Task UpdateAsync_WhenCalled_UpdateCorrectly()
        {
            var song = new SongDto { Id = Guid.NewGuid(), Name = "test", Links = new() };
            await _songServiceImp.UpdateAsync(song);
            _songRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Song>(s => s.Name == "test")));
            _linkServiceMock.Verify(l => l.UpdateLinksAsync(song.Id.Value, song.Links));
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_DeleteCorrectly()
        {
            var id = Guid.NewGuid();
            await _songServiceImp.DeleteAsync(id);
            _songRepositoryMock.Verify(r => r.DeleteAsync(id));
        }
    }
}
