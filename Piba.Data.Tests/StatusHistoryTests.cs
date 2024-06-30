using Piba.Data.Entities;

namespace Piba.Data.Tests
{
    public class StatusHistoryTests
    {
        [Fact]
        public void StatusHistory_WhenConstructed_PopulatePropertiesCorrectly()
        {
            var history = new StatusHistory();
            var aMonthAgo = DateTime.UtcNow.AddMonths(-1);
            Assert.Equal(aMonthAgo.Month, history.Month);
            Assert.Equal(aMonthAgo.Year, history.Year);
        }
    }
}
