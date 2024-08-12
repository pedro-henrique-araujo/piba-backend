using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Piba.Services.Interfaces;

[Route("google-login")]
[ApiController]
public class GoogleLoginController : ControllerBase
{
    private readonly GoogleLoginService _googleLoginService;

    public GoogleLoginController(UserManager<IdentityUser> userManager, IConfiguration configuration, GoogleLoginService googleLoginService)
    {
        _googleLoginService = googleLoginService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> LoginAsync([FromHeader] string authorization)
    {
        var token = await _googleLoginService.LoginAsync(authorization);

        return Ok(new { token });
    }
}
