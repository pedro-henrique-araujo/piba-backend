using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Piba.Services.Interfaces;

namespace Piba.Function
{
    public class MemberStatusReviewer
    {
        private readonly LogService _logService;
        private readonly MemberService _memberService;
        private readonly EmailService _emailService;

        public MemberStatusReviewer(LogService logService, MemberService memberService, EmailService emailService)
        {
            _logService = logService;
            _memberService = memberService;
            _emailService = emailService;
        }

        [Function(nameof(ReviewMembersActivityTimerTriggerAsync))]
        public async Task ReviewMembersActivityTimerTriggerAsync(
            [TimerTrigger("* * * * *", RunOnStartup = true)] TimerInfo myTimer)
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
            await FinalNotificationsAsync();
        }

        private async Task FinalNotificationsAsync()
        {
            await _logService.LogMessageAsync($"{nameof(ReviewMembersActivityAsync)} executed at: {DateTime.Now}");
            _emailService.SendEmailToDeveloper(new()
            {
                Subject = "Members Activity Review",
                Body = "PIBA Members Activity is Reviewed"
            });

        }
    }
}
