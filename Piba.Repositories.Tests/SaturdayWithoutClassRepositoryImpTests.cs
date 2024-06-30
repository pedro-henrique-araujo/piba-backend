using Piba.Data;
using Piba.Data.Entities;

namespace Piba.Repositories.Tests
{
    public class SaturdayWithoutClassRepositoryImpTests : IDisposable
    {
        private readonly PibaDbContext _pibaDbContext;
        private readonly SaturdayWithoutClassRepositoryImp _saturdayWithoutClassRepository;

        public SaturdayWithoutClassRepositoryImpTests()
        {
            _pibaDbContext = Common.GenerateInMemoryDatabase(nameof(SaturdayWithoutClassRepositoryImpTests));
            _saturdayWithoutClassRepository = new SaturdayWithoutClassRepositoryImp(_pibaDbContext);
        }

        [Fact]
        public async Task DateWouldHaveClassAsync_DateHasClass_ReturnsFalse()
        {
            var date = new DateTime(2021, 1, 1);
            _pibaDbContext.Set<SaturdayWithoutClass>()
                .Add(new() { Name = "A", DateTime = date });
            await _pibaDbContext.SaveChangesAsync();

            var result = await _saturdayWithoutClassRepository.DateWouldHaveClassAsync(date);

            Assert.False(result);
        }

        [Fact]
        public async Task DateWouldHaveClassAsync_DateHasNoClass_ReturnsTrue()
        {
            var date = new DateTime(2021, 1, 1);
            var result = await _saturdayWithoutClassRepository.DateWouldHaveClassAsync(date);

            Assert.True(result);
        }

        public void Dispose()
        {
            _pibaDbContext.Database.EnsureDeleted();
            _pibaDbContext.Dispose();
        }
    }
}
