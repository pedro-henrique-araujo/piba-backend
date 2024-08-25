namespace Piba.Data.Entities
{
    public class SessionAttendanceItem
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public Member Member { get; set; }
        public Guid SessionAttedanceId { get; set; }
        public SessionAttendance SessionAttendance { get; set; }
    }
}
