using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piba.Data.Dto;
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

        [HttpGet]
        public async Task<IActionResult> PaginateAsync([FromQuery] PaginationQueryParameters paginationQueryParameters)
        {
            var output = await _memberService.PaginateAsync(paginationQueryParameters);
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpOptions]
        public async Task<IActionResult> GetMemberOptions()
        {
            var options = await _memberService.GetOptionsAsync();
            return Ok(options);
        }

    }
}
