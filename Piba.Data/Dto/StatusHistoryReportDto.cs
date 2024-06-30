using Piba.Data.Enums;

namespace Piba.Data.Dto
{
    public class StatusHistoryReportDto
    {
        public string Name { get; set; }
        public MemberStatus Status { get; set; }
        public int Count { get; set; }
    }
}
