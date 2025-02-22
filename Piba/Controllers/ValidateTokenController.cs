using Microsoft.AspNetCore.Mvc;

namespace Piba.Controllers
{
    [Route("validate-token")]
    [ApiController]
    public class ValidateTokenController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
