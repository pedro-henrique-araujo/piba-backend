using Microsoft.AspNetCore.Identity;
using Piba.Data.Entities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Piba.Repositories
{
    public interface AuthenticationRepository
    {
        Task<PibaUser?> ApplyGoogleAuthenticationAsync(Payload googlePayload);
        Task<PibaUser?> ApplyGoogleAuthenticationAsync(Payload googlePayload, string role);
        Task<PibaUser?> FindByGoogleKeyAsync(Payload googlePayload);
    }
}