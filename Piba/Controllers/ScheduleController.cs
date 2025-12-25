using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;

namespace Piba.Controllers
{
    [Route("Schedule")]
    [Authorize(Roles = "cante")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly PibaDbContext db;

        public ScheduleController(PibaDbContext db)
        {
            this.db = db;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var schedule = await db.Schedules.Include(s => s.ScheduleSongs)
                    .ThenInclude(ss => ss.Song).ThenInclude(s => s.Links)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        [HttpPost("{songId}")]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid songId)
        {
            var schedule = new Schedule
            {
                ScheduleSongs = new() { new() { SongId = songId } }
            };

            await db.Schedules.AddAsync(schedule);
            await db.SaveChangesAsync();
            return Created("/", schedule);
        }

        [HttpPatch("{id}/song/{songId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromRoute] Guid songId)
        {
            var schedule = await db.Schedules
                .Include(s => s.ScheduleSongs)
                .FirstOrDefaultAsync(s => s.Id == id);
            schedule?.ScheduleSongs.Add(new() { SongId = songId });

            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}/song/{songId}/remove")]
        public async Task<IActionResult> RemoveAsync([FromRoute] Guid id, [FromRoute] Guid songId)
        {
            var schedule = await db.Schedules
                .Include(s => s.ScheduleSongs)
                .FirstOrDefaultAsync(s => s.Id == id);

            schedule?.ScheduleSongs.RemoveAll(s => s.SongId == songId);

            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
