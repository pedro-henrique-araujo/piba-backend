using Piba.Data.Enums;

namespace Piba.Data.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? RecurrentExcuse { get; set; }
        public MemberStatus Status { get; set; }
        public DateTime LastStatusUpdate {  get; set; }
    }
}
