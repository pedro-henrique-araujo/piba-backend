using Piba.Data.Entities;
using Piba.Data.Enums;

namespace Piba.Data.Tests
{
    public class StatusHistoryItemTests
    {
        [Fact]
        public void StatusHistoryItem_WhenConstructedWithMember_FillOutFieldsCorrectly()
        {
            var today = DateTime.Today;
            var member = new Member
            {
                Id = Guid.NewGuid(),
                Status = MemberStatus.Inactive,
            };
            var history = new StatusHistoryItem(member);
            Assert.Equal(member.Id, history.MemberId);
            Assert.Equal(member.Status, history.Status);
        }
    }
}
