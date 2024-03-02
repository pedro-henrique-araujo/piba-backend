using Moq;
using Piba.Repositories.Interfaces;

namespace Piba.Services.Tests
{
    public class SaturdayWithoutClassServiceImpTests
    {
        private readonly Mock<SaturdayWithoutClassRepository> _repositoryMock;
        private readonly SaturdayWithoutClassServiceImp _saturdayWithoutClassRepository;

        public SaturdayWithoutClassServiceImpTests()
        {
            _repositoryMock = new Mock<SaturdayWithoutClassRepository>();
            _saturdayWithoutClassRepository = new SaturdayWithoutClassServiceImp(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetLastThreeClassesDatesAsync_WhenNoSaturdayWithoutClass_ReturnCorrectly()
        {
            var lastSaturday = GetLastSaturday();
            _repositoryMock.Setup(r => r.DateHasClassAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var days = await _saturdayWithoutClassRepository.GetLastThreeClassesDatesAsync();

            Assert.Equal(lastSaturday, days[0]);
            Assert.Equal(lastSaturday.AddDays(-7), days[1]);
            Assert.Equal(lastSaturday.AddDays(-14), days[2]);
        }

        [Fact]
        public async Task GetLastThreeClassesDatesAsync_WhenLastHadNoClasses_ReturnCorrectly()
        {
            var lastSaturday = GetLastSaturday();
            _repositoryMock.Setup(r => r.DateHasClassAsync(It.Is<DateTime>(d => d == lastSaturday)))
                .ReturnsAsync(false);

            _repositoryMock.Setup(r => r.DateHasClassAsync(It.Is<DateTime>(d => d != lastSaturday)))
                .ReturnsAsync(true);

            var days = await _saturdayWithoutClassRepository.GetLastThreeClassesDatesAsync();

            Assert.Equal(lastSaturday.AddDays(-7), days[0]);
            Assert.Equal(lastSaturday.AddDays(-14), days[1]);
            Assert.Equal(lastSaturday.AddDays(-21), days[2]);
        }


        [Fact]
        public async Task GetLastThreeClassesDatesAsync_WhenTwoWeeksBeforeLastSaturdayHadNoClasses_ReturnCorrectly()
        {
            var lastSaturday = GetLastSaturday();
            _repositoryMock.Setup(r => r.DateHasClassAsync(It.Is<DateTime>(d => d == lastSaturday.AddDays(-7))))
                .ReturnsAsync(false);
            _repositoryMock.Setup(r => r.DateHasClassAsync(It.Is<DateTime>(d => d != lastSaturday.AddDays(-7))))
               .ReturnsAsync(true);

            var days = await _saturdayWithoutClassRepository.GetLastThreeClassesDatesAsync();

            Assert.Equal(lastSaturday, days[0]);
            Assert.Equal(lastSaturday.AddDays(-14), days[1]);
            Assert.Equal(lastSaturday.AddDays(-21), days[2]);
        }

        private static DateTime GetLastSaturday()
        {
            var today = DateTime.Today;
            return today.AddDays(-1 - (double)today.DayOfWeek);
        }
    }
}
