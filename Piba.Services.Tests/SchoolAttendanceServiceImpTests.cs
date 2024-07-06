using Moq;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class SchoolAttendanceServiceImpTests
    {
        private readonly Mock<EnvironmentVariables> _environmentVariablesMock;
        private readonly Mock<SchoolAttendanceRepository> _repositoryMock;
        private readonly Mock<SaturdayWithoutClassService> _saturdayWithoutClassServiceMock;
        private readonly SchoolAttendanceServiceImp _schoolAttendanceService;

        public SchoolAttendanceServiceImpTests()
        {
            _environmentVariablesMock = new Mock<EnvironmentVariables>();
            _repositoryMock = new Mock<SchoolAttendanceRepository>();
            _saturdayWithoutClassServiceMock = new Mock<SaturdayWithoutClassService>();
            _schoolAttendanceService = new SchoolAttendanceServiceImp(
                _environmentVariablesMock.Object,
                _repositoryMock.Object,
                _saturdayWithoutClassServiceMock.Object);
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
            var minValidTime = TimeSpan.Parse("01:00");
            _environmentVariablesMock.Setup(m => m.MinValidTime).Returns(minValidTime);
            var maxValidTime = TimeSpan.Parse("02:00");
            _environmentVariablesMock.Setup(m => m.MaxValidTime).Returns(maxValidTime);

            var classesDates = new List<DateTime>();
            _saturdayWithoutClassServiceMock.Setup(m => m.GetLastThreeClassesDatesAsync())
                   .ReturnsAsync(classesDates);


            var memberId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByDatesAsync(It.Is<MemberAttendancesByDatesFilter>(m =>
                        m.MemberId == memberId
                        && m.Dates == classesDates
                        && m.MinValidTime == minValidTime
                        && m.MaxValidTime == maxValidTime
                    )))
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
            var minValidTime = TimeSpan.Parse("01:00");
            _environmentVariablesMock.Setup(m => m.MinValidTime).Returns(minValidTime);
            var maxValidTime = TimeSpan.Parse("02:00");
            _environmentVariablesMock.Setup(m => m.MaxValidTime).Returns(maxValidTime);

            var classesDates = new List<DateTime>();
            _saturdayWithoutClassServiceMock.Setup(m => m.GetLastThreeClassesDatesAsync())
                   .ReturnsAsync(classesDates);

            var memberId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByDatesAsync(It.Is<MemberAttendancesByDatesFilter>(m =>
                        m.MemberId == memberId
                        && m.Dates == classesDates
                        && m.MinValidTime == minValidTime
                        && m.MaxValidTime == maxValidTime
                    )))
                .ReturnsAsync(count);

            var result = await _schoolAttendanceService.MemberMissedAnyOfLastThreeClassesAsync(memberId);
            Assert.Equal(expectedResult, result);
        }
    }
}
