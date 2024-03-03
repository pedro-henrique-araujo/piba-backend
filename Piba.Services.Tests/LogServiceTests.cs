using Moq;
using Piba.Repositories.Interfaces;

namespace Piba.Services.Tests
{
    public class LogServiceTests
    {
        private readonly Mock<LogRepository> _repositoryMock;
        private readonly LogServiceImp _logService;

        public LogServiceTests()
        {
            _repositoryMock = new Mock<LogRepository>();
            _logService = new LogServiceImp(_repositoryMock.Object);
        }

        [Fact]
        public async Task LogMessageAsync_WhenCalled_CallRepositoryMethod()
        {
            await _logService.LogMessageAsync("abc");
            _repositoryMock.Verify(r => r.LogMessageAsync("abc"));
        }
    }
}
