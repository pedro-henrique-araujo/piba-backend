using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;
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
        private readonly SchoolAttendanceRepository _schoolAttendanceRepository;
        private readonly EnvironmentVariables _environmentVariables;
        private readonly ExcelService _excelService;

        public StatusHistoryServiceImp(
            StatusHistoryRepository statusHistoryRepository,
            MemberRepository memberRepository,
            StatusHistoryItemRepository statusHistoryItemRepository,
            EmailService emailService,
            SchoolAttendanceRepository schoolAttendanceRepository,
            EnvironmentVariables environmentVariables,
            ExcelService excelService)
        {
            _statusHistoryRepository = statusHistoryRepository;
            _memberRepository = memberRepository;
            _statusHistoryItemRepository = statusHistoryItemRepository;
            _emailService = emailService;
            _schoolAttendanceRepository = schoolAttendanceRepository;
            _environmentVariables = environmentVariables;
            _excelService = excelService;
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
            var excelFile = await _excelService.GenerateStatusHistoryAsync();
            _emailService.SendEmailToDeveloper(new() { Subject = "File" }, excelFile);
        }
    }
}
