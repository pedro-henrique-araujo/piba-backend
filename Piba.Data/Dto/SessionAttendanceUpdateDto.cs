
namespace Piba.Data.Dto
{
    public class SessionAttendanceUpdateDto
    {
        public DateTime? DateTime { get; set; }

        public List<SessionAttendanceItemUpdateDto> Items { get; set; }
        public Guid Id { get; set; }
    }
}
