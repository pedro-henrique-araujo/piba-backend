namespace Piba.Data.Dto
{
    public class MemberAttendancesByDatesFilter
    {
        public Guid MemberId { get; set; }
        public List<DateTime> Dates { get; set; }
        public TimeSpan MinValidTime {  get; set; }
        public TimeSpan MaxValidTime { get; set; }
    }
}
