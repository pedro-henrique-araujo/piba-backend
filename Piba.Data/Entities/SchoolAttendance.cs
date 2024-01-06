namespace Piba.Data.Entities
{
    public class SchoolAttendance
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public bool IsPresent { get; set; }
        public string? Excuse { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
