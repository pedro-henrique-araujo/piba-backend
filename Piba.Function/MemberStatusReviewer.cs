using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Piba.Services.Interfaces;

namespace Piba.Function
{
    public class MemberStatusReviewer
    {
        private readonly LogService _logService;
        private readonly MemberService _memberService;

        public MemberStatusReviewer(LogService logService, MemberService memberService)
        {
            _logService = logService;
            _memberService = memberService;
        }

        [Function(nameof(ReviewMembersActivityTimerTriggerAsync))]
        public async Task ReviewMembersActivityTimerTriggerAsync(
            [TimerTrigger("0 0 * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            await ReviewMembersActivityAsync();
        }

        [Function(nameof(ReviewMembersActivityHttpTriggerAsync))]
        public async Task ReviewMembersActivityHttpTriggerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            await ReviewMembersActivityAsync();
        }

        public async Task ReviewMembersActivityAsync()
        {
            await _memberService.ReviewMembersActivityAsync();
            await FinalLogAsync();
        }

        private async Task FinalLogAsync()
        {
            await _logService.LogMessageAsync($"{nameof(ReviewMembersActivityAsync)} executed at: {DateTime.Now}");
        }
    }
}
