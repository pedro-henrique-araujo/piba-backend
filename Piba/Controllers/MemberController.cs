using Microsoft.AspNetCore.Mvc;
using Piba.Services.Interfaces;

namespace Piba.Controllers
{
    [Route("member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private MemberService _memberService;

        public MemberController(MemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpOptions]
        public async Task<IActionResult> GetMemberOptions()
        {
            var options = await _memberService.GetOptionsAsync();
            return Ok(options);
        }

    }
}
