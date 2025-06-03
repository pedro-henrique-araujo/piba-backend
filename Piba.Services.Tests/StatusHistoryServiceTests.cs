using Moq;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class StatusHistoryServiceTests
    {
        private readonly Mock<StatusHistoryRepository> _statusHistoryRepositoryMock;
        private readonly Mock<MemberRepository> _memberRepositoryMock;
        private readonly Mock<StatusHistoryItemRepository> _statusHistoryItemRepositoryMock;
        private readonly Mock<EmailService> _emailServiceMock;
        private readonly Mock<SchoolAttendanceRepository> _schoolAttendanceRepositoryMock;
        private readonly Mock<EnvironmentVariables> _environmentVariablesMock;
        private readonly Mock<ExcelService> _excelServiceMock;
        private readonly StatusHistoryServiceImp _memberStatusHistoryService;

        public StatusHistoryServiceTests()
        {
            _statusHistoryRepositoryMock = new Mock<StatusHistoryRepository>();
            _memberRepositoryMock = new Mock<MemberRepository>();
            _statusHistoryItemRepositoryMock = new Mock<StatusHistoryItemRepository>();
            _emailServiceMock = new Mock<EmailService>();
            _schoolAttendanceRepositoryMock = new Mock<SchoolAttendanceRepository>();
            _environmentVariablesMock = new Mock<EnvironmentVariables>();
            _excelServiceMock = new Mock<ExcelService>();
            _memberStatusHistoryService = new StatusHistoryServiceImp(
                _statusHistoryRepositoryMock.Object,
                _memberRepositoryMock.Object,
                _statusHistoryItemRepositoryMock.Object,
                _emailServiceMock.Object,
                _schoolAttendanceRepositoryMock.Object,
                _environmentVariablesMock.Object,
                _excelServiceMock.Object
                );
        }

        [Fact]
        public async Task CreateForLastMonthIfItDoesNotExistAsync_HistoryExists_DoNotCreateANewOne()
        {
            _statusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
                .ReturnsAsync(true);
            await _memberStatusHistoryService.CreateForLastMonthIfItDoesNotExistAsync();
            _statusHistoryRepositoryMock.Verify(r =>
                r.CreateAsync(It.IsAny<StatusHistory>()),
                    Times.Never);

            _statusHistoryItemRepositoryMock.Verify(r =>
                r.CreateAsync(It.IsAny<IEnumerable<StatusHistoryItem>>()),
                    Times.Never);
        }

        [Fact]
        public async Task CreateForLastMonthIfItDoesNotExistAsync_HistoryDoesNotExist_CreateANewOne()
        {
            var members = new List<Member>
            {
                new(), new(), new()
            };

            _statusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
                .ReturnsAsync(false);

            _memberRepositoryMock.Setup(m => m.GetAllInactiveAndActiveAsync())
                .ReturnsAsync(members);

            await _memberStatusHistoryService.CreateForLastMonthIfItDoesNotExistAsync();

            _statusHistoryRepositoryMock.Verify(r =>
                r.CreateAsync(It.Is<StatusHistory>(s => s.Id != Guid.Empty && s.IsSent == false)),
                Times.Once);

            _statusHistoryItemRepositoryMock.Verify(r =>
                r.CreateAsync(It.Is<IEnumerable<StatusHistoryItem>>(e =>
                    e.Count() == members.Count() && e.All(i => i.StatusHistoryId != Guid.Empty))), Times.Once);
        }

        [Fact]
        public async Task SendStatusHistoryEmailToReceiversAsync_WhenNotSent_SendFile()
        {

            _statusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
                .ReturnsAsync(true);

            _statusHistoryRepositoryMock.Setup(r => r.IsHistoryOfLastMonthSentAsync())
                .ReturnsAsync(false);

            var fakeExcelFile = new byte[] { 1, 2, 3, 4, 5 };
            var attachmentReportFakeFile = new byte[] { 1, 2, 3, 4, 5, 6 };

            _excelServiceMock.Setup(s => s.GenerateStatusHistoryAsync())
                .ReturnsAsync(fakeExcelFile);

            var baseDate = DateTime.Today;

            _excelServiceMock.Setup(s => s.GenerateAttendanceReportAsync(baseDate))
                .ReturnsAsync(attachmentReportFakeFile);

            await _memberStatusHistoryService.SendStatusHistoryEmailToReceiversAsync(baseDate);

            _statusHistoryRepositoryMock.Verify(r =>
                r.HistoryForLastMonthExistsAsync(),
                    Times.Once);


            var name = $"Atividade de Membros {DateTime.Now.AddMonths(-1):MM/yyyy}.xlsx";
            _emailServiceMock.Verify(r =>
                r.SendEmailToDeveloper(It.Is<SendEmailDto>(e => 
                    e.Subject == name), 
                    It.Is<AttachmentDto>(a => a.Name == name && a.Bytes == fakeExcelFile),
                    It.Is<AttachmentDto>(a => a.Name == "Atividade por dia no mês anterior" && a.Bytes == attachmentReportFakeFile)),
                Times.Once);

            _statusHistoryRepositoryMock.Verify(r =>
                r.MarkLastMonthHistoryAsSentAsync(),
                Times.Once);
        }

        [Fact]
        public async Task SendStatusHistoryEmailToReceivers_WhenSent_DontSendFile()
        {
            _statusHistoryRepositoryMock.Setup(m => m.HistoryForLastMonthExistsAsync())
               .ReturnsAsync(true);

            _statusHistoryRepositoryMock.Setup(r => r.IsHistoryOfLastMonthSentAsync())
                .ReturnsAsync(true);

            await _memberStatusHistoryService.SendStatusHistoryEmailToReceiversAsync(DateTime.Today);

            _emailServiceMock.Verify(s => 
                s.SendEmailToDeveloper(It.IsAny<SendEmailDto>(), It.IsAny<AttachmentDto>()), 
                Times.Never);
        }
    }
}
