
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piba.Data.Dto;
using Piba.Services.Interfaces;

namespace Piba.Controllers
{
    [Route("session-attendance")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class SessionAttendanceController : ControllerBase
    {
        private readonly SessionAttendanceService _sessionAttendanceService;

        public SessionAttendanceController(SessionAttendanceService sessionAttendanceService)
        {
            _sessionAttendanceService = sessionAttendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> PaginateAsync([FromRoute] PaginationQueryParameters paginationQueryParameters)
        {
            var output = await _sessionAttendanceService.PaginateAsync(paginationQueryParameters);
            return Ok(output);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var output = await _sessionAttendanceService.GetByIdAsync(id);
            return Ok(output);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] SessionAttendanceDto sessionAttendanceDto)
        {
            await _sessionAttendanceService.CreateAsync(sessionAttendanceDto);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] SessionAttendanceUpdateDto sessionAttendanceUpdateDto)
        {
            await _sessionAttendanceService.UpdateAsync(sessionAttendanceUpdateDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _sessionAttendanceService.DeleteAsync(id);
            return Ok();
        }
    }
}
