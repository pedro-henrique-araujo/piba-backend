using Microsoft.AspNetCore.Identity;

namespace Piba.Data.Entities
{
    public class CanteAvailability
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public PibaUser? User { get; set; }
        public string UserId { get; set; }
    }
}
