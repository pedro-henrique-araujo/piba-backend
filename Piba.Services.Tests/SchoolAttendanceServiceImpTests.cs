using Moq;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Services.Tests
{
    public class SchoolAttendanceServiceImpTests
    {
        private readonly Mock<SchoolAttendanceRepository> _repositoryMock;
        private readonly SchoolAttendanceServiceImp _schoolAttendanceService;

        public SchoolAttendanceServiceImpTests()
        {
            _repositoryMock = new Mock<SchoolAttendanceRepository>();
            _schoolAttendanceService = new SchoolAttendanceServiceImp(_repositoryMock.Object);
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
