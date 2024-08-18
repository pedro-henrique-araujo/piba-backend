using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Piba.Services
{
    public class GoogleWebSignatureServiceImp : GoogleWebSignatureService
    {
        private readonly IConfiguration _configuration;

        public GoogleWebSignatureServiceImp(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string googleIdentityToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _configuration["Google:ClientId"] }
            };

            return await GoogleJsonWebSignature.ValidateAsync(googleIdentityToken, settings);
        }
    }
}
