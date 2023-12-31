namespace Piba.Data.Entities
{
    public class SchoolAttendance
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public bool IsPresent { get; set; }
        public string? Excuse { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
