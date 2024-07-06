namespace Piba.Data.Entities
{
    public class SchoolAttendance
    {
        public Guid Id { get; private set; }
        public Guid MemberId { get; set; }
        public Member Member { get; set; }
        public bool IsPresent { get; set; }
        public string? Excuse { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? CreatedDate { get; set; }

        public void Create()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }
}
