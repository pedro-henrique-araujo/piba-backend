using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Piba.Data.Entities;
using Piba.Services.Interfaces;

[Route("google-login")]
[ApiController]
public class GoogleLoginController : ControllerBase
{
    private readonly GoogleLoginService _googleLoginService;

    public GoogleLoginController(GoogleLoginService googleLoginService)
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

    [AllowAnonymous]
    [HttpGet("{role}")]
    public async Task<IActionResult> LoginAsync([FromHeader] string authorization, string role)
    {
        var token = await _googleLoginService.LoginOrCreateAsync(authorization, role);

        return Ok(new { token });
    }
}
