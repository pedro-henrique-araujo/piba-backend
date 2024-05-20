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
        private readonly MemberStatusHistoryService _memberStatusHistoryService;

        public MemberStatusReviewer(
            LogService logService, 
            MemberService memberService,
            EmailService emailService,
            MemberStatusHistoryService memberStatusHistoryService)
        {
            _logService = logService;
            _memberService = memberService;
            _emailService = emailService;
            _memberStatusHistoryService = memberStatusHistoryService;
        }

        [Function(nameof(ReviewMembersActivityTimerTriggerAsync))]
        public async Task ReviewMembersActivityTimerTriggerAsync(
            [TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            await ReviewMembersActivityAsync();
        }

        [Function(nameof(ReviewMembersActivityHttpTriggerAsync))]
        public async Task ReviewMembersActivityHttpTriggerAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            await ReviewMembersActivityAsync();
        }

        [Function(nameof(SendMemberStatusHistoryEmailToReceiversAsync))]
        public async Task SendMemberStatusHistoryEmailToReceiversAsync([TimerTrigger("0 0 0 0 * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            await _memberStatusHistoryService.SendMemberStatusHistoryEmailToReceiversAsync();
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
