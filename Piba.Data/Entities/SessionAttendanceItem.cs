namespace Piba.Data.Entities
{
    public class SessionAttendanceItem
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public Member? Member { get; set; }
        public Guid SessionAttendanceId { get; set; }
        public SessionAttendance? SessionAttendance { get; set; }
        public bool IsPresent { get; set; }
    }
}
