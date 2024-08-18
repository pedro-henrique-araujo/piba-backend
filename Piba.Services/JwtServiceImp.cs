using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Piba.Repositories;
using Piba.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Piba.Services
{
    public class JwtServiceImp : JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly AuthorizationRepository _authorizationRepository;

        public JwtServiceImp(IConfiguration configuration, AuthorizationRepository authorizationRepository)
        {
            _configuration = configuration;
            _authorizationRepository = authorizationRepository;
        }

        public async Task<string> GenerateUserTokenAsync(IdentityUser user)
        {
            var securityToken = await GenerateJwtSecurityTokenAsync(user);

            return EncodeAsString(securityToken);
        }

        private async Task<JwtSecurityToken> GenerateJwtSecurityTokenAsync(IdentityUser user)
        {
            var claims = await GenerateClaimsAsync(user);

            var creds = GenerateCredentials();

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return token;
        }

        private string EncodeAsString(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GenerateClaimsAsync(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var rolesList = await _authorizationRepository.GetUserRolesAsync(user);

            var roleClaims = rolesList.Select(r => new Claim(ClaimTypes.Role, r));

            claims.AddRange(roleClaims);
            return claims;
        }

        private SigningCredentials GenerateCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return creds;
        }
    }
}
