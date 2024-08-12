using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> LoginWithGoogle([FromHeader] string identityToken)
    {
        var payload = await ValidateGoogleToken(identityToken);
        if (payload == null)
        {
            return BadRequest("Invalid Google token.");
        }

        var info = new UserLoginInfo("Google", payload.Subject, "Google");
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user == null)
        {
            user = new IdentityUser { UserName = payload.Email, Email = payload.Email };
            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, info);
        }

        var tokenString = await GenerateJwtToken(user.Email, user);
        return Ok(new { Token = tokenString });
    }

    public async Task<string> GenerateJwtToken(string email, IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { "197419869969-9n5carqvk5rlavebv124kq0u9qr4fe9b.apps.googleusercontent.com" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            return payload;
        }
        catch (InvalidJwtException ex)
        {
            return null;
        }
    }

}
