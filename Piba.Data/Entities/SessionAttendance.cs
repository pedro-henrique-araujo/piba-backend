namespace Piba.Data.Entities
{
    public class SessionAttendance
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public List<SessionAttendanceItem> SessionAttendanceItems { get; set; }
    }
}
