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

        [Function(nameof(ReviewMembersActivityAsync))]
        public async Task ReviewMembersActivityAsync(
            [TimerTrigger("0 0 * * 0,4", RunOnStartup = true)] TimerInfo myTimer)
        {
            await _memberService.ReviewMembersActivityAsync();
            await FinalLogAsync(myTimer);
        }

        private async Task FinalLogAsync(TimerInfo myTimer)
        {
            await _logService.LogMessageAsync($"{nameof(ReviewMembersActivityAsync)} Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                await _logService.LogMessageAsync($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
