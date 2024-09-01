using Microsoft.Azure.Functions.Worker;
using Piba.Services.Interfaces;
using System.Threading;

namespace Piba.Function
{
    public class StatusHistorySender
    {
        private readonly StatusHistoryService _statusHistoryService;
        private readonly Semaphore _semaphore;

        public StatusHistorySender(StatusHistoryService statusHistoryService, Semaphore semaphore)
        {
            _statusHistoryService = statusHistoryService;
            _semaphore = semaphore;
        }

        [Function(nameof(StatusHistoryService))]
        public async Task SendStatusHistory(
            [TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo timerInfo)
        {
            _semaphore.WaitOne();
            await _statusHistoryService.SendStatusHistoryEmailToReceiversAsync();
            _semaphore.Release();
        }
    }
}
