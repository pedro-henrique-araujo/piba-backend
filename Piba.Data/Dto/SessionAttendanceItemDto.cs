
namespace Piba.Data.Dto
{
    public class SessionAttendanceItemDto
    {
        public Guid SessionAttendanceId { get; set; }
        public Guid MemberId { get; set; }
        public bool IsPresent { get; set; }
    }
}