using Moq;
using OfficeOpenXml;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services.Tests
{
    public class ExcelServiceImpTests
    {
        private readonly Mock<EnvironmentVariables> _environmentVariablesMock;
        private readonly Mock<StatusHistoryItemRepository> _statusHistoryItemRepositoryMock;
        private readonly Mock<MemberRepository> _memberRepositoryMock;
        private readonly Mock<SchoolAttendanceRepository> _schoolAttendanceRepositoryMock;
        private readonly ExcelServiceImp _excelService;
        private readonly DateTime _baseDate;

        public ExcelServiceImpTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _environmentVariablesMock = new Mock<EnvironmentVariables>();
            _statusHistoryItemRepositoryMock = new Mock<StatusHistoryItemRepository>();
            _memberRepositoryMock = new Mock<MemberRepository>();
            _schoolAttendanceRepositoryMock = new Mock<SchoolAttendanceRepository>();
            _excelService = new ExcelServiceImp(
                    _environmentVariablesMock.Object,
                    _statusHistoryItemRepositoryMock.Object,
                    _memberRepositoryMock.Object,
                    _schoolAttendanceRepositoryMock.Object
                );
            _baseDate = DateTime.UtcNow;
        }

        [Fact]
        public async Task GenerateStatusHistoryAsync_WhenCalled_GenerateCorrectly()
        {
            SetupGetLastHistory();
            SetupGetAllAlwaysExcused();
            SetupGetLastMonthsExcuses();

            var file = await _excelService.GenerateStatusHistoryAsync();
            var package = Common.LoadExcelPackage(file);
            AssertLastHistory(package);
            AssertAlwaysExcused(package);
            AssertLastMonthsExcuses(package);
        }

        private void AssertLastHistory(ExcelPackage package)
        {
            var inactiveWorksheet = package.Workbook.Worksheets[0];
            Assert.Equal("Inativos", inactiveWorksheet.Name);
            Assert.Equal("Nome", inactiveWorksheet.Cells["B2"].GetValue<string>());
            Assert.Equal("Número de presenças ou justificativas no mês passado", inactiveWorksheet.Cells["C2"].GetValue<string>());
            Assert.Equal("A", inactiveWorksheet.Cells["B3"].GetValue<string>());
            Assert.Equal(1, inactiveWorksheet.Cells["C3"].GetValue<int>());
            Assert.Equal("B", inactiveWorksheet.Cells["B4"].GetValue<string>());
            Assert.Equal(1, inactiveWorksheet.Cells["C4"].GetValue<int>());

            var activeWorksheet = package.Workbook.Worksheets[1];

            Assert.Equal("Ativos", activeWorksheet.Name);
            Assert.Equal("Nome", activeWorksheet.Cells["B2"].GetValue<string>());
            Assert.Equal("Número de presenças ou justificativas no mês passado", activeWorksheet.Cells["C2"].GetValue<string>());
            Assert.Equal("AB", activeWorksheet.Cells["B3"].GetValue<string>());
            Assert.Equal(2, activeWorksheet.Cells["C3"].GetValue<int>());
            Assert.Equal("BC", activeWorksheet.Cells["B4"].GetValue<string>());
            Assert.Equal(2, activeWorksheet.Cells["C4"].GetValue<int>());
        }

        private void AssertAlwaysExcused(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets[2];
            Assert.Equal("Sempre Justificados", worksheet.Name);
            Assert.Equal("Nome", worksheet.Cells["B2"].GetValue<string>());
            Assert.Equal("Justificativa", worksheet.Cells["C2"].GetValue<string>());
            Assert.Equal("A", worksheet.Cells["B3"].GetValue<string>());
            Assert.Equal("B", worksheet.Cells["C3"].GetValue<string>());
            Assert.Equal("C", worksheet.Cells["B4"].GetValue<string>());
            Assert.Equal("D", worksheet.Cells["C4"].GetValue<string>());
        }

        private void AssertLastMonthsExcuses(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets[3];
            Assert.Equal("Justificativas", worksheet.Name);
            Assert.Equal("Nome", worksheet.Cells["B2"].GetValue<string>());
            Assert.Equal("Motivo", worksheet.Cells["C2"].GetValue<string>());
            Assert.Equal("A", worksheet.Cells["B3"].GetValue<string>());
            Assert.Equal("B", worksheet.Cells["C3"].GetValue<string>());
            Assert.Equal(_baseDate.ToString("dd/MM/yyyy HH:mm"),
                worksheet.Cells["D3"].GetValue<string>());
            Assert.Equal("C", worksheet.Cells["B4"].GetValue<string>());
            Assert.Equal("D", worksheet.Cells["C4"].GetValue<string>());
            Assert.Equal(_baseDate.AddDays(-1).ToString("dd/MM/yyyy HH:mm"),
                worksheet.Cells["D4"].GetValue<string>());
        }

        private void SetupGetLastHistory()
        {
            var environmentVariables = _environmentVariablesMock.Object;
            _statusHistoryItemRepositoryMock.Setup(r => r.GetLastHistoryAsync(
                     It.Is<ValidTimeFilter>(f => f.MinValidTime == environmentVariables.MinValidTime
                        && f.MaxValidTime == environmentVariables.MaxValidTime)))
                .ReturnsAsync(new List<StatusHistoryReportDto>
                {
                    new ()
                    {
                        Name = "B",
                        Count = 1,
                        Status = MemberStatus.Inactive
                    },
                    new ()
                    {
                        Name = "BC",
                        Count = 2,
                        Status = MemberStatus.Active
                    },
                    new ()
                    {
                        Name = "A",
                        Count = 1,
                        Status = MemberStatus.Inactive
                    },
                    new ()
                    {
                        Name = "AB",
                        Count = 2,
                        Status = MemberStatus.Active
                    }
                });
        }

        private void SetupGetAllAlwaysExcused()
        {
            _memberRepositoryMock.Setup(r => r.GetAllAlwaysExcusedAsync())
                .ReturnsAsync(new List<Member>
                {
                    new()
                    {
                        Name = "C",
                        RecurrentExcuse = "D"
                    },
                     new()
                    {
                        Name = "A",
                        RecurrentExcuse = "B"
                    },
                });
        }

        private void SetupGetLastMonthsExcuses()
        {
            _schoolAttendanceRepositoryMock.Setup(r => r.GetLastMonthExcusesAsync())
                .ReturnsAsync(new List<SchoolAttendance>
                {
                    new()
                    {
                        Member = new()
                        {
                            Name = "C"
                        },
                        Excuse = "D",
                        CreatedDate = _baseDate.AddDays(-1)
                    },
                    new()
                    {
                        Member = new()
                        {
                            Name = "A"
                        },
                        Excuse = "B",
                        CreatedDate = _baseDate
                    },
                });
        }
    }
}
