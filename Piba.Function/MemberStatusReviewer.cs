using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Piba.Services.Interfaces;

namespace Piba.Function
{
    public class MemberStatusReviewer
    {
        private readonly ILogger _logger;
        private readonly MemberService _memberService;

        public MemberStatusReviewer(ILoggerFactory loggerFactory, MemberService memberService)
        {
            _logger = loggerFactory.CreateLogger<MemberStatusReviewer>();
            _memberService = memberService;
        }

        [Function(nameof(ReviewMembersActivityAsync))]
        public async Task ReviewMembersActivityAsync(
            [TimerTrigger("0 0 * * 0,4", RunOnStartup = true)] TimerInfo myTimer)
        {
            await _memberService.ReviewMembersActivityAsync();
            FinalLog(myTimer);
        }

        private void FinalLog(TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
