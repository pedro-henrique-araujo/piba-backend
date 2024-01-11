using Piba.Data.Entities;

namespace Piba.Data.Tests
{
    public class SchoolAttendanceTests
    {
        [Fact]
        public void Create_WhenCalled_CreateIdAndUpdateCreationDate()
        {
            var attendance = new SchoolAttendance();

            attendance.Create();

            Assert.NotEqual(Guid.Empty, attendance.Id);
            Assert.Equal(DateTime.Today, attendance.CreatedDate?.Date);
        }
    }
}