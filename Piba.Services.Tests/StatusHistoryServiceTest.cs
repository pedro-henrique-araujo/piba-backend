using Moq;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class StatusHistoryServiceTest
    {
        private readonly Mock<StatusHistoryRepository> _statusHistoryRepositoryMock;
        private readonly Mock<MemberRepository> _memberRepositoryMock;
        private readonly Mock<StatusHistoryItemRepository> _statusHistoryItemRepositoryMock;
        private readonly Mock<EmailService> _emailServiceMock;
        private readonly StatusHistoryServiceImp _memberStatusHistoryService;

        public StatusHistoryServiceTest()
        {
            _statusHistoryRepositoryMock = new Mock<StatusHistoryRepository>();
            _memberRepositoryMock = new Mock<MemberRepository>();
            _statusHistoryItemRepositoryMock = new Mock<StatusHistoryItemRepository>();
            _emailServiceMock = new Mock<EmailService>();
            _memberStatusHistoryService = new StatusHistoryServiceImp(
                _statusHistoryRepositoryMock.Object, 
                _memberRepositoryMock.Object, 
                _statusHistoryItemRepositoryMock.Object,
                _emailServiceMock.Object
                );
        }

        [Fact]
        public async Task CreateForLastMonthIfItDoesNotExistAsync_HistoryExists_DoNotCreateANewOne()
        {
            _statusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
                .ReturnsAsync(true);
            await _memberStatusHistoryService.CreateForLastMonthIfItDoesNotExistAsync();
            _statusHistoryRepositoryMock.Verify(r => 
                r.CreateAsync(It.IsAny<StatusHistory>()), 
                    Times.Never);

            _statusHistoryItemRepositoryMock.Verify(r => 
                r.CreateAsync(It.IsAny<IEnumerable<StatusHistoryItem>>()), 
                    Times.Never);
        }

        [Fact]
        public async Task CreateForLastMonthIfItDoesNotExistAsync_HistoryDoesNotExist_CreateANewOne()
        {
            var members = new List<Member>
            {
                new(), new(), new()
            };

            _statusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
                .ReturnsAsync(false);

            _memberRepositoryMock.Setup(m => m.GetAllInactiveAndActiveAsync())
                .ReturnsAsync(members);

            await _memberStatusHistoryService.CreateForLastMonthIfItDoesNotExistAsync();

            _statusHistoryRepositoryMock.Verify(r => 
                r.CreateAsync(It.Is<StatusHistory>(s => s.Id != Guid.Empty && s.IsSent == false)), 
                Times.Once);

            _statusHistoryItemRepositoryMock.Verify(r => 
                r.CreateAsync(It.Is<IEnumerable<StatusHistoryItem>>(e => 
                    e.Count() == members.Count() && e.All(i => i.StatusHistoryId != Guid.Empty))), Times.Once);
        }
    }
}
