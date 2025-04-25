using Moq;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Services.Tests
{
    public class SongServiceImpTests
    {
        private readonly Mock<SongRepository> _songRepositoryMock;
        private readonly SongServiceImp _songServiceImp;

        public SongServiceImpTests()
        {
            _songRepositoryMock = new Mock<SongRepository>();
            _songServiceImp = new SongServiceImp(_songRepositoryMock.Object);
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
            var song = new Song();
            var id = Guid.NewGuid();

            _songRepositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(song);

            var result = await _songServiceImp.GetByIdAsync(id);

            Assert.Equal(song, result);

        }

        [Fact]
        public async Task CreateAsync_WhenCalled_CreateCorrectly()
        {
            var song = new Song();
            await _songServiceImp.CreateAsync(song);

            _songRepositoryMock.Verify(r => r.CreateAsync(song));
        }

        [Fact]
        public async Task UpdateAsync_WhenCalled_UpdateCorrectly()
        {
            var song = new Song();
            await _songServiceImp.UpdateAsync(song);
            _songRepositoryMock.Verify(r => r.UpdateAsync(song));
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
