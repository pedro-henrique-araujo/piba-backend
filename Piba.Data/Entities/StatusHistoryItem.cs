using Piba.Data.Enums;

namespace Piba.Data.Entities
{
    public class StatusHistoryItem
    {
        public Guid Id { get; set; }
        public Guid StatusHistoryId { get; set; }
        public StatusHistory StatusHistory { get; set; }
        public Guid MemberId { get; set; }
        public Member Member { get; set; }
        public MemberStatus Status { get; set; }
        public StatusHistoryItem()
        {

        }

        public StatusHistoryItem(Member member)
        {
            Status = member.Status;
            MemberId = member.Id;
        }
    }
}
