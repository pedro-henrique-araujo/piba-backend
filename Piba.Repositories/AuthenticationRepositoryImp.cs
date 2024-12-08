using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Piba.Data.Entities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Piba.Repositories
{
    public class AuthenticationRepositoryImp : AuthenticationRepository
    {
        private readonly UserManager<PibaUser> _userManager;

        public AuthenticationRepositoryImp(UserManager<PibaUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PibaUser?> ApplyGoogleAuthenticationAsync(Payload googlePayload)
        {
            var user = await _userManager.FindByEmailAsync(googlePayload.Email);
            var info = new UserLoginInfo("Google", googlePayload.Subject, "Google");
            await _userManager.AddLoginAsync(user, info);
            return user;
        }

        public async Task<PibaUser?> ApplyGoogleAuthenticationAsync(Payload googlePayload, string role)
        {
            var user = await _userManager.FindByEmailAsync(googlePayload.Email);
            if (user is null)
            {
                var newUser = new PibaUser
                {
                    Email = googlePayload.Email,
                    NormalizedEmail = googlePayload.Email.ToUpper(),
                    UserName = googlePayload.Email,
                    NormalizedUserName = googlePayload.Email.ToUpper(),
                    Name = googlePayload.Name,
                    PhotoUrl = googlePayload.Picture
                };
                await _userManager.CreateAsync(newUser);
                user = newUser;
            }

            var info = new UserLoginInfo("Google", googlePayload.Subject, "Google");
            await _userManager.AddLoginAsync(user, info);
            return user;
        }

        public async Task<PibaUser?> FindByGoogleKeyAsync(Payload googlePayload)
        {
            var info = new UserLoginInfo("Google", googlePayload.Subject, "Google");
            return await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        }
    }
}
