using Microsoft.Azure.Functions.Worker;
using Piba.Services.Interfaces;

namespace Piba.Function
{
    public class StatusHistorySender
    {
        private readonly StatusHistoryService _statusHistoryService;

        public StatusHistorySender(StatusHistoryService statusHistoryService)
        {
            _statusHistoryService = statusHistoryService;
        }

        [Function(nameof(StatusHistoryService))]
        public async Task SendStatusHistory(
            [TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo timerInfo)
        {
            await _statusHistoryService.SendStatusHistoryEmailToReceiversAsync();
        }
    }
}
