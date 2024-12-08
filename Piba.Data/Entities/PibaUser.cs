using Microsoft.AspNetCore.Identity;

namespace Piba.Data.Entities
{
    public class PibaUser : IdentityUser
    {
        public string? PhotoUrl { get; set; }
        public string? Name { get; set; }
    }
}
