using Microsoft.AspNetCore.Identity;

namespace Piba.Repositories
{
    public class AuthorizationRepositoryImp : AuthorizationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorizationRepositoryImp(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }
    }
}
