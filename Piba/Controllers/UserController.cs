using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piba.Services.Interfaces;

namespace Piba.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync()
        {
            return Created();
        }

        [HttpOptions]
        public async Task<IActionResult> GetOptionsAsync()
        {
            var options = await _userService.GetUserOptionsAsync();
            return Ok(options);
        }
    }
}
