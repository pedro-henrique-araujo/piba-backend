using Piba.Data.Entities;

namespace Piba.Data.Dto
{
    public class SessionAttendanceDto
    {
        public DateTime? DateTime { get; set; }

        public List<SessionAttendanceItemDto> Items { get; set; }
    }
}
