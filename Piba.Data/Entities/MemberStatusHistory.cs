using Piba.Data.Enums;

namespace Piba.Data.Entities
{
    public class MemberStatusHistory
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public MemberStatus Status { get; set; }
        public DateTime HistoryMonth { get; set; }
        public MemberStatusHistory()
        {

        }

        public MemberStatusHistory(Member member)
        {
            var today = DateTime.Today;
            HistoryMonth = today.AddDays(1 - today.Day).AddMonths(-1);
            Status = member.Status;
            MemberId = member.Id;
        }
    }
}
