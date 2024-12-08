using Microsoft.AspNetCore.Identity;
using Piba.Data.Entities;

namespace Piba.Repositories
{
    public class AuthorizationRepositoryImp : AuthorizationRepository
    {
        private readonly UserManager<PibaUser> _userManager;

        public AuthorizationRepositoryImp(UserManager<PibaUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> GetUserRolesAsync(PibaUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }
    }
}
