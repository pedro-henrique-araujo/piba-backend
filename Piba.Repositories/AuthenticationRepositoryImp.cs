using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Piba.Repositories
{
    public class AuthenticationRepositoryImp : AuthenticationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationRepositoryImp(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser?> ApplyGoogleAuthenticationAsync(Payload googlePayload)
        {
            var user = await _userManager.FindByEmailAsync(googlePayload.Email);
            var info = new UserLoginInfo("Google", googlePayload.Subject, "Google");
            await _userManager.AddLoginAsync(user, info);
            return user;
        }

        public async Task<IdentityUser?> FindByGoogleKeyAsync(Payload googlePayload)
        {
            var info = new UserLoginInfo("Google", googlePayload.Subject, "Google");
            return await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        }
    }
}
