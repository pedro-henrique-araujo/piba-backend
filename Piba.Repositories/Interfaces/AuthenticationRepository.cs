using Microsoft.AspNetCore.Identity;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Piba.Repositories
{
    public interface AuthenticationRepository
    {
        Task<IdentityUser?> ApplyGoogleAuthenticationAsync(Payload googlePayload);
        Task<IdentityUser?> FindByGoogleKeyAsync(Payload googlePayload);
    }
}