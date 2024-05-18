using Piba.Data.Enums;

namespace Piba.Data.Entities
{
    public class MemberStatusHistory
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public MemberStatus Status { get; set; }
        public DateTime HistoryMonth {  get; set; }
    }
}
