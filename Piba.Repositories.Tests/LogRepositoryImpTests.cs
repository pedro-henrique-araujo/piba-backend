using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;

namespace Piba.Repositories.Tests
{
    public class LogRepositoryImpTests : IDisposable
    {
        private readonly PibaDbContext _pibaContext;
        private readonly LogRepositoryImp _logRepository;

        public LogRepositoryImpTests()
        {
            _pibaContext = Common.GenerateInMemoryDatabase(nameof(LogRepositoryImpTests));
            _logRepository = new LogRepositoryImp(_pibaContext);
        }

        [Fact]
        public async Task LogMessageAsync_WhenCalled_CreateMessage()
        {
            await _logRepository.LogMessageAsync("A");
            var messageCreated = await _pibaContext.Set<Log>()
                .AnyAsync(l => l.Message == "A");
            Assert.True(messageCreated);
        }

        public void Dispose()
        {
            _pibaContext.Database.EnsureDeleted();
            _pibaContext.Dispose();
        }
    }
}