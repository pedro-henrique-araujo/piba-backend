using Moq;
using Piba.Data.Dto;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class MemberServiceImpTests
    {
        private readonly Mock<MemberRepository> _repositoryMock;
        private readonly Mock<SchoolAttendanceService> _schoolAttendanceServiceMock;
        private readonly MemberServiceImp _memberService;

        public MemberServiceImpTests()
        {
            _repositoryMock = new Mock<MemberRepository>();
            _schoolAttendanceServiceMock = new Mock<SchoolAttendanceService>();
            _memberService = new MemberServiceImp(_repositoryMock.Object, _schoolAttendanceServiceMock.Object);
                
        }

        [Fact]
        public async Task GetOptionsAsync_WhenCalled_ReturnOptionsCorretly()
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

        public List<MemberOptionDto> GetMockedMembers()
        {
            return new()
            {
                new () { Id = Guid.NewGuid(), Name = "abc" },
                new () { Id = Guid.NewGuid(), Name = "def" }
            };
        }
    }
}