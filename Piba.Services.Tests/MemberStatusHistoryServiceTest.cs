using Moq;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Services.Tests
{
    public class MemberStatusHistoryServiceTest
    {
        private readonly Mock<MemberStatusHistoryRepository> _memberStatusHistoryRepositoryMock;
        private readonly Mock<MemberRepository> _memberRepositoryMock;
        private readonly MemberStatusHistoryServiceImp _memberStatusHistoryService;

        public MemberStatusHistoryServiceTest()
        {
            _memberStatusHistoryRepositoryMock = new Mock<MemberStatusHistoryRepository>();
            _memberRepositoryMock = new Mock<MemberRepository>();
            _memberStatusHistoryService = new MemberStatusHistoryServiceImp(_memberStatusHistoryRepositoryMock.Object, _memberRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateActivityHistoryForLastMonthIfItDoesNotExistAsync_HistoryExists_DoNotCreateANewOne()
        {
            _memberStatusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
                .ReturnsAsync(true);
            await _memberStatusHistoryService.CreateActivityHistoryForLastMonthIfItDoesNotExistAsync();
            _memberStatusHistoryRepositoryMock.Verify(r => 
                r.CreateAsync(It.IsAny<IEnumerable<MemberStatusHistory>>()), 
                    Times.Never);
        }

        [Fact]
        public async Task CreateActivityHistoryForLastMonthIfItDoesNotExistAsync_HistoryDoesNotExist_CreateANewOne()
        {
            var members = new List<Member>
            {
                new(), new(), new()
            };

            _memberStatusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
                .ReturnsAsync(false);

            _memberRepositoryMock.Setup(m => m.GetAllInactiveAndActiveAsync())
                .ReturnsAsync(members);

            await _memberStatusHistoryService.CreateActivityHistoryForLastMonthIfItDoesNotExistAsync();

            _memberStatusHistoryRepositoryMock.Verify(r => 
                r.CreateAsync(It.Is<IEnumerable<MemberStatusHistory>>(e => 
                    e.Count() == members.Count())), Times.Once);
        }
    }
}
