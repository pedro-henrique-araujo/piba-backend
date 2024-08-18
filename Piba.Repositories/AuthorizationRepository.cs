using Microsoft.AspNetCore.Identity;

namespace Piba.Repositories
{
    public interface AuthorizationRepository
    {
        Task<IList<string>> GetUserRolesAsync(IdentityUser user);
    }
}