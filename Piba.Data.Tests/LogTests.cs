using Piba.Data.Entities;

namespace Piba.Data.Tests
{
    public class LogTests
    {
        [Fact]
        public void Log_WhenCreated_FillCorrectly()
        {
            var log = new Log("abc");
            Assert.Equal("abc", log.Message);
            Assert.Equal(DateTime.Today, log.Created.Date);
            Assert.Equal(DateTime.Now.Hour, log.Created.Hour);
            Assert.Equal(DateTime.Now.Minute, log.Created.Minute);
        }
    }
}
