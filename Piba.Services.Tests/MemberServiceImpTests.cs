using Moq;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class MemberServiceImpTests
    {
        private readonly Mock<MemberRepository> _repositoryMock;
        private readonly Mock<SchoolAttendanceService> _schoolAttendanceServiceMock;
        private readonly Mock<StatusHistoryService> _memberStatusHistoryServiceMock;
        private readonly MemberServiceImp _memberService;
        private readonly DateTime _baseDate;

        public MemberServiceImpTests()
        {
            _repositoryMock = new Mock<MemberRepository>();
            _schoolAttendanceServiceMock = new Mock<SchoolAttendanceService>();
            _memberStatusHistoryServiceMock = new Mock<StatusHistoryService>();
            _memberService = new MemberServiceImp(_repositoryMock.Object, _schoolAttendanceServiceMock.Object, _memberStatusHistoryServiceMock.Object);
            _baseDate = DateTime.UtcNow;

        }

        [Fact]
        public async Task GetOptionsAsync_WhenCalled_ReturnOptionsCorrectly()
        {
            var mockedMembers = GetMockedMembers();

            _repositoryMock.Setup(r => r.GetAllInactiveAndActiveOptionsAsync())
                .ReturnsAsync(mockedMembers);

            var memberOptions = await _memberService.GetOptionsAsync();

            Assert.Equal(mockedMembers[0].Id, memberOptions[0].Id);
            Assert.Equal(mockedMembers[0].Name, memberOptions[0].Name);
            Assert.Equal(mockedMembers[1].Name, memberOptions[1].Name);
            Assert.Equal(mockedMembers[1].Name, memberOptions[1].Name);

        }

        [Fact]
        public async Task ReviewMembersActivityAsync_WhenCalled_ReviewCorrectly()
        {
            var initiallyActive = GetMockedActiveMembers();
            var initiallyInactive = GetMockedInactiveMembers();

            SetupForActiveMembers(initiallyActive);

            SetupForInactiveMembers(initiallyInactive);

            await _memberService.ReviewMembersActivityAsync();
            _memberStatusHistoryServiceMock.Verify(r => r.CreateForLastMonthIfItDoesNotExistAsync(), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Single(initiallyActive.Where(a => a.Status == MemberStatus.Active));
            Assert.Equal(2, initiallyActive.Where(a => a.LastStatusUpdate > _baseDate).Count());
            Assert.Single(initiallyInactive.Where(a => a.Status == MemberStatus.Inactive));
            Assert.Equal(2, initiallyInactive.Where(a => a.LastStatusUpdate > _baseDate).Count());

        }

        private void SetupForInactiveMembers(List<Member> initiallyInactive)
        {
            _repositoryMock.Setup(r => r.GetAllInactiveAsync()).ReturnsAsync(initiallyInactive);

            _schoolAttendanceServiceMock
                .Setup(s => s.MemberMissedAnyOfLastThreeClassesAsync(It.Is<Guid>(id => id == initiallyInactive.First().Id)))
                .ReturnsAsync(false);

            _schoolAttendanceServiceMock
                .Setup(s => s.MemberMissedAnyOfLastThreeClassesAsync(It.Is<Guid>(id => id == initiallyInactive.First().Id)))
                .ReturnsAsync(true);
        }

        private void SetupForActiveMembers(List<Member> initiallyActive)
        {
            _repositoryMock.Setup(r => r.GetAllActiveCreatedBefore21DaysAgoAsync()).ReturnsAsync(initiallyActive);
            _schoolAttendanceServiceMock
                .Setup(s => s.MemberIsPresentAtLeastOnceOnLastThreeClassesAsync(It.Is<Guid>(id => id == initiallyActive.First().Id)))
                .ReturnsAsync(false);

            _schoolAttendanceServiceMock
                .Setup(s => s.MemberIsPresentAtLeastOnceOnLastThreeClassesAsync(It.Is<Guid>(id => id == initiallyActive.First().Id)))
                .ReturnsAsync(true);
        }

        private List<MemberOptionDto> GetMockedMembers()
        {
            return new()
            {
                new () { Id = Guid.NewGuid(), Name = "abc" },
                new () { Id = Guid.NewGuid(), Name = "def" }
            };
        }

        private List<Member> GetMockedActiveMembers()
        {
            return new()
            {
                new () { Id = Guid.NewGuid(), Status = MemberStatus.Active, LastStatusUpdate = _baseDate},
                new () { Id = Guid.NewGuid(), Status = MemberStatus.Active, LastStatusUpdate = _baseDate},
                new () { Id = Guid.NewGuid(), Status = MemberStatus.Active, LastStatusUpdate = _baseDate}
            };
        }

        private List<Member> GetMockedInactiveMembers()
        {
            return new()
            {
                new() { Id = Guid.NewGuid(), Status = MemberStatus.Inactive, LastStatusUpdate = _baseDate},
                new() { Id = Guid.NewGuid(), Status = MemberStatus.Inactive, LastStatusUpdate = _baseDate},
                new() { Id = Guid.NewGuid(), Status = MemberStatus.Inactive, LastStatusUpdate = _baseDate}
            };
        }
    }
}