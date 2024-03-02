using Moq;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class SchoolAttendanceServiceImpTests
    {
        private readonly Mock<SchoolAttendanceRepository> _repositoryMock;
        private readonly Mock<SaturdayWithoutClassService> _saturdayWithoutClassServiceMock;
        private readonly SchoolAttendanceServiceImp _schoolAttendanceService;

        public SchoolAttendanceServiceImpTests()
        {
            _repositoryMock = new Mock<SchoolAttendanceRepository>();
            _saturdayWithoutClassServiceMock = new Mock<SaturdayWithoutClassService>();
            _schoolAttendanceService = new SchoolAttendanceServiceImp(_repositoryMock.Object, _saturdayWithoutClassServiceMock.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_CreateAttendance()
        {
            var attendance = new SchoolAttendance();

            await _schoolAttendanceService.CreateAsync(attendance);

            _repositoryMock.Verify(r => r.CreateAsync(attendance));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public async Task MemberIsPresentAtLeastOnceOnLastThreeClassesAsync_WhenCalled_ReturnCorrectly(int count, bool expectedResult)
        {
            var memberId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByDatesAsync(It.Is<MemberClassesByDatesFilter>(m => m.MemberId == memberId)))
                .ReturnsAsync(count);

            var result = await _schoolAttendanceService.MemberIsPresentAtLeastOnceOnLastThreeClassesAsync(memberId);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        public async Task MemberMissedAnyOfLastThreeClassesAsync_WhenCalled_ReturnCorrectly(int count, bool expectedResult)
        {
            var memberId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByDatesAsync(It.Is<MemberClassesByDatesFilter>(m => m.MemberId == memberId)))
                .ReturnsAsync(count); ;

            var result = await _schoolAttendanceService.MemberMissedAnyOfLastThreeClassesAsync(memberId);
            Assert.Equal(expectedResult, result);
        }
    }
}
