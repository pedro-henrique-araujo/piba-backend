using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piba.Data.Entities;
using Piba.Services.Interfaces;

namespace Piba.Controllers
{
    [Route("school-attendance")]
    [ApiController]
    public class SchoolAttendanceController : ControllerBase
    {
        private SchoolAttendanceService _schoolAttendanceService;

        public SchoolAttendanceController(SchoolAttendanceService schoolAttendanceService)
        {
            _schoolAttendanceService = schoolAttendanceService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(SchoolAttendance schoolAttendance)
        {
            await _schoolAttendanceService.CreateAsync(schoolAttendance);
            return Created("/", null);
        }
    }
}
