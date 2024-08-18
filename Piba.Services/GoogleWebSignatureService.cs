using Google.Apis.Auth;

namespace Piba.Services
{
    public interface GoogleWebSignatureService
    {
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string googleIdentityToken);
    }
}