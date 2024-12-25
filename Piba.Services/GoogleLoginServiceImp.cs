using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Piba.Data.Entities;
using Piba.Repositories;

namespace Piba.Services.Interfaces
{
    public class GoogleLoginServiceImp : GoogleLoginService
    {
        private readonly UserManager<PibaUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;
        private readonly GoogleWebSignatureService _googleWebSignatureService;
        private readonly AuthenticationRepository _authenticationRepository;

        public GoogleLoginServiceImp(
            UserManager<PibaUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, 
            JwtService jwtService, 
            GoogleWebSignatureService googleWebSignatureService, 
            AuthenticationRepository authenticationRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtService = jwtService;
            _googleWebSignatureService = googleWebSignatureService;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<string> LoginAsync(string googleIdentityToken)
        {
            var googlePayload = await _googleWebSignatureService.ValidateGoogleToken(googleIdentityToken);

            var user = await _authenticationRepository.FindByGoogleKeyAsync(googlePayload);

            if (user is null)
            {
                user = await _authenticationRepository.ApplyGoogleAuthenticationAsync(googlePayload);
            }

            return await _jwtService.GenerateUserTokenAsync(user);
        }

        public async Task<string> LoginOrCreateAsync(string googleIdentityToken, string role)
        {
            var googlePayload = await _googleWebSignatureService.ValidateGoogleToken(googleIdentityToken);

            var user = await _authenticationRepository.FindByGoogleKeyAsync(googlePayload);

            if (user is null)
            {
                user = await _authenticationRepository.ApplyGoogleAuthenticationAsync(googlePayload, role);
            }

            user.PhotoUrl = googlePayload.Picture;
            await _userManager.UpdateAsync(user);
            await _roleManager.CreateAsync(new() { Name = role });
            await _userManager.AddToRoleAsync(user, role);
            return await _jwtService.GenerateUserTokenAsync(user);
        }
    }
}
