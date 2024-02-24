using Moq;
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
    }
}
