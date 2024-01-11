using Moq;
using Piba.Data.Dto;
using Piba.Repositories.Interfaces;

namespace Piba.Services.Tests
{
    public class MemberServiceImpTests
    {
        private readonly Mock<MemberRepository> _repositoryMock;
        private readonly MemberServiceImp _memberService;

        public MemberServiceImpTests()
        {
            _repositoryMock = new Mock<MemberRepository>();
            _memberService = new MemberServiceImp(_repositoryMock.Object);
                
        }

        [Fact]
        public async Task GetOptionsAsync_WhenCalled_ReturnOptionsCorretly()
        {
            var mockedMembers = GetMockedMembers();

            _repositoryMock.Setup(r => r.GetAllInactiveAndActiveAsync())
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