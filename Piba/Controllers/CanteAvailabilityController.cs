using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using System.Security.Claims;

namespace Piba.Controllers
{
    [Route("cante-availability")]
    [ApiController]
    public class CanteAvailabilityController : ControllerBase
    {
        private readonly PibaDbContext _dbContext;
        private readonly UserManager<PibaUser> _userManager;

        public CanteAvailabilityController(PibaDbContext dbContext, UserManager<PibaUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] DateRangeWithEmailDto dateRangeWithEmail)
        {
            var availabilitiesQueryable = _dbContext.CanteAvailabilites.AsQueryable();

            if (dateRangeWithEmail.Start.HasValue)
            {
                availabilitiesQueryable = availabilitiesQueryable.Where(a => a.Date.Date >= dateRangeWithEmail.Start.Value.Date);
            }

            if (dateRangeWithEmail.End.HasValue)
            {
                availabilitiesQueryable = availabilitiesQueryable.Where(a => a.Date.Date <= dateRangeWithEmail.End.Value.Date);
            }

            if (dateRangeWithEmail.Id is not null)
            {
                availabilitiesQueryable = availabilitiesQueryable.Where(a => a.UserId == dateRangeWithEmail.Id);
            }

            availabilitiesQueryable = availabilitiesQueryable.Include(a => a.User);

            var list = await availabilitiesQueryable.Select(a => new CanteAvailabilityDto(a)).ToListAsync();

            return Ok(list);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserAvailabilities(CanteAvailabilitiesDto userAvailability)
        {
            var a = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userInDb = await _userManager.FindByEmailAsync(a);

            var availabilityInDb = await _dbContext.CanteAvailabilites
                .Where(a => a.Date.Date >= userAvailability.DateRange.Start.Date
                    && a.Date.Date <= userAvailability.DateRange.End.Date
                    && a.UserId == userInDb.Id)
                .ToListAsync();

            var availabilityDatesInDb = availabilityInDb.Select(a => a.Date);

            var addedAvailabilities = userAvailability.Availabilities
                .Where(a => availabilityDatesInDb.Contains(a) == false)
                .Select(a => new CanteAvailability
                {
                    Date = a.Date,
                    User = userInDb
                });

            _dbContext.CanteAvailabilites.AddRange(addedAvailabilities);

            var removedAvailabilities = availabilityInDb
                .Where(a => userAvailability.Availabilities.Contains(a.Date) == false);

            _dbContext.CanteAvailabilites.RemoveRange(removedAvailabilities);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
