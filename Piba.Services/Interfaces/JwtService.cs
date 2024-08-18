using Microsoft.AspNetCore.Identity;

namespace Piba.Services.Interfaces
{
    public interface JwtService
    {
        Task<string> GenerateUserTokenAsync(IdentityUser user);
    }
}