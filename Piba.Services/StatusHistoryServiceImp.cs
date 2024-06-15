using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class StatusHistoryServiceImp : StatusHistoryService
    {
        private readonly StatusHistoryRepository _statusHistoryRepository;
        private readonly MemberRepository _memberRepository;
        private readonly StatusHistoryItemRepository _statusHistoryItemRepository;
        private readonly EmailService _emailService;

        public StatusHistoryServiceImp(
            StatusHistoryRepository statusHistoryRepository,
            MemberRepository memberRepository,
            StatusHistoryItemRepository statusHistoryItemRepository,
            EmailService emailService)
        {
            _statusHistoryRepository = statusHistoryRepository;
            _memberRepository = memberRepository;
            _statusHistoryItemRepository = statusHistoryItemRepository;
            _emailService = emailService;
        }

        public async Task CreateForLastMonthIfItDoesNotExistAsync()
        {
            if (await _statusHistoryRepository.HistoryForLastMonthExistsAsync()) return;

            var members = await _memberRepository.GetAllInactiveAndActiveAsync();

            var historyId = Guid.NewGuid();

            await _statusHistoryRepository.CreateAsync(new() { Id = historyId });

            var histories = members.Select(m => new StatusHistoryItem(m)
            {
                StatusHistoryId = historyId,
            });

            await _statusHistoryItemRepository.CreateAsync(histories);
        }

        public async Task SendStatusHistoryEmailToReceiversAsync()
        {
            await CreateForLastMonthIfItDoesNotExistAsync();
            using var excelWrapper = new ExcelWrapper();
            excelWrapper.AddWorksheet<StatusHistoryItem>(new("test")
            {
                Map = new()
                {
                    ["Nome"] = r => r.MemberId
                },
                Rows = new() { new() { MemberId = Guid.NewGuid() } }
            });
            var excelFile = await excelWrapper.GetByteArrayAsync();
            _emailService.SendEmailToDeveloper(new() { Subject = "File" }, excelFile);
        }
    }
}
