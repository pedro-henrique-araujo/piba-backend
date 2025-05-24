
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Services.Interfaces;

namespace Piba.Controllers
{
    [Route("song")]
    [Authorize(Roles = "cante")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly SongService _songService;

        public SongController(SongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAsync([FromQuery] PaginationQueryParameters paginationQueryParameters)
        {
            var output = await _songService.PaginateAsync(paginationQueryParameters);
            return Ok(output);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var output = await _songService.GetByIdAsync(id);
            return Ok(output);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Song song)
        {
            await _songService.CreateAsync(song);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Song song)
        {
            await _songService.UpdateAsync(song);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _songService.DeleteAsync(id);
            return Ok();
        }
    }
}
