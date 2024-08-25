
namespace Piba.Data.Dto
{
    public class SessionAttendanceUpdateDto
    {
        public List<SessionAttendanceItemUpdateDto> Items { get; set; }
        public Guid Id { get; set; }
    }
}
