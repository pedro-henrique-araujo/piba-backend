namespace Piba.Data.Entities
{
    public class MediaAvailability
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public PibaUser? User { get; set; }
        public string UserId { get; set; }
    }
}
