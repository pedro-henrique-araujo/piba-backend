using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piba.Data.Dto;
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

        [HttpGet]
        public async Task<IActionResult> PaginateAsync([FromQuery] PaginationQueryParameters paginationQueryParameters)
        {
            var output = await _userService.PaginateAsync(paginationQueryParameters);
            return Ok(output);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync()
        {
            return Created();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UserDto userUpdateDto)
        {
            await _userService.UpdateAsync(id, userUpdateDto);

            return Ok();
        }


        [HttpOptions]
        public async Task<IActionResult> GetOptionsAsync()
        {
            var options = await _userService.GetUserOptionsAsync();
            return Ok(options);
        }
    }
}
