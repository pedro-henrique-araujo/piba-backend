using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piba.Data.Dto;
using Piba.Services.Interfaces;

namespace Piba.Controllers
{    
    [Route("role")]
    [Authorize]
    [ApiController]

    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAsync([FromQuery] PaginationQueryParameters paginationQueryParameters)
        {
            var output = await _roleService.PaginateAsync(paginationQueryParameters);
            return Ok(output);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoleDto roleDto)
        {
            await _roleService.CreateAsync(roleDto.Name);
            return Created();
        }

        [HttpOptions]
        public async Task<IActionResult> GetOptionsAsync()
        {
            var options = await _roleService.GetRoleOptionsAsync();
            return Ok(options);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _roleService.DeleteAsync(id);
            return NoContent();
        }
    }
}
