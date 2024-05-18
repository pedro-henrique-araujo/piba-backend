namespace Piba.Data.Dto
{
    public class MemberClassesByDatesFilter
    {
        public Guid MemberId { get; set; }
        public List<DateTime> Dates { get; set; }
        public TimeSpan MinValidTime {  get; set; }
        public TimeSpan MaxValidTime { get; set; }
    }
}
