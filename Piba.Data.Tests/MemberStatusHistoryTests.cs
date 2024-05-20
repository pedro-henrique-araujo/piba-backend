using Piba.Data.Entities;
using Piba.Data.Enums;

namespace Piba.Data.Tests
{
    public class MemberStatusHistoryTests
    {
        [Fact]
        public void MemberStatusHistory_WhenConstructedWithMember_FillOutFieldsCorrectly()
        {
            var today = DateTime.Today;
            var expectedHistoryMonth = today.AddDays(1 - today.Day).AddMonths(-1);
            var member = new Member
            {
                Id = Guid.NewGuid(),
                Status = MemberStatus.Inactive,
            };
            var history = new MemberStatusHistory(member);
            Assert.Equal(member.Id, history.MemberId);
            Assert.Equal(member.Status, history.Status);
            Assert.Equal(expectedHistoryMonth, history.HistoryMonth);
        }
    }
}
