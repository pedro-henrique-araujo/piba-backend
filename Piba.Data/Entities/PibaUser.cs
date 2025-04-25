using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Piba.Data.Entities
{
    public class PibaUser : IdentityUser
    {
        public string? PhotoUrl { get; set; }
        public string? Name { get; set; }
    }
}
