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

        [Fact]
        public async Task GenerateAttendanceReportAsync_WhenFourSaturdays_GenerateCorrectly()
        {
            var report = GetFourSaturdaysAttendanceReportDictionary();
            var timespan = new TimeSpan(1);

            _environmentVariablesMock.Setup(c => c.MaxValidTime).Returns(timespan);
            _environmentVariablesMock.Setup(c => c.TimezoneOffset).Returns(-1);

            _schoolAttendanceRepositoryMock
                .Setup(r => r.GetAttendancesReportAsync(It.Is<List<DateOnly>>(
                    l =>
                    l.Count == 4 &&
                    l.All(l => l.Year == 2024 && l.Month == 12 && l.DayOfWeek == DayOfWeek.Saturday) &&
                    l[0].Day == 7 &&
                    l[1].Day == 14 &&
                    l[2].Day == 21 &&
                    l[3].Day == 28
                 ), timespan, -1))
                .ReturnsAsync(report);


            var today = new DateTime(2025, 1, 2);

            var file = await _excelService.GenerateAttendanceReportAsync(today);

            var package = Common.LoadExcelPackage(file);

            AssertFourSaturdaysAttendancesReport(package);
        }



        [Fact]
        public async Task GenerateAttendanceReportAsync_WhenFiveSaturdays_GenerateCorrectly()
        {
            var report = GetFiveSaturdaysAttendanceReportDicionary();
            var timespan = new TimeSpan(1);
            _environmentVariablesMock.Setup(c => c.MaxValidTime).Returns(timespan);
            _environmentVariablesMock.Setup(c => c.TimezoneOffset).Returns(-1);

            _schoolAttendanceRepositoryMock
            .Setup(r => r.GetAttendancesReportAsync(It.Is<List<DateOnly>>(
                l =>
                l.Count == 5 &&
                l.All(l => l.Year == 2025 && l.Month == 5 && l.DayOfWeek == DayOfWeek.Saturday) &&
                l[0].Day == 3 &&
                l[1].Day == 10 &&
                l[2].Day == 17 &&
                l[3].Day == 24 &&
                l[4].Day == 31
             ), timespan, -1))
            .ReturnsAsync(report);

            var today = new DateTime(2025, 6, 3);

            var file = await _excelService.GenerateAttendanceReportAsync(today.Date);

            var package = Common.LoadExcelPackage(file);
            AssertFiveSaturdaysAttendancesReport(package);

        }

        private Dictionary<DateOnly, List<AttendanceReportDto>> GetFourSaturdaysAttendanceReportDictionary()
        {
            return new Dictionary<DateOnly, List<AttendanceReportDto>>
            {
                [new DateOnly(2024, 12, 7)] = new List<AttendanceReportDto>
                {
                    new() { Name = "B", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)) },
                    new() { Name = "A", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) }
                },
                [new DateOnly(2024, 12, 14)] = new List<AttendanceReportDto>()
                {
                    new()  { Name = "C", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)) },
                    new()  { Name = "D", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(11)) }
                },
                [new DateOnly(2024, 12, 21)] = new List<AttendanceReportDto>{
                    new() { Name = "E", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(12)) },
                    new() { Name = "F", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(13)) }
                },
                [new DateOnly(2024, 12, 28)] = new List<AttendanceReportDto>
                {
                    new() { Name = "G", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)) },
                    new() { Name = "H", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(15)) }
                }
            };
        }

        private void AssertFourSaturdaysAttendancesReport(ExcelPackage package)
        {
            var worksheets = package.Workbook.Worksheets;

            Assert.All(worksheets, w =>
            {
                Assert.Equal("Nome", w.Cells["B2"].GetValue<string>());
                Assert.Equal("Horário", w.Cells["C2"].GetValue<string>());
            });

            Assert.Equal("7", worksheets[0].Name);
            AssertWorksheetTwoRowsTable(worksheets[0], "A", "08:00", "B", "09:00");

            Assert.Equal("14", worksheets[1].Name);
            AssertWorksheetTwoRowsTable(worksheets[1], "C", "10:00", "D", "11:00");

            Assert.Equal("21", worksheets[2].Name);
            AssertWorksheetTwoRowsTable(worksheets[2], "E", "12:00", "F", "13:00");

            Assert.Equal("28", worksheets[3].Name);
            AssertWorksheetTwoRowsTable(worksheets[3], "G", "14:00", "H", "15:00");
        }

        private Dictionary<DateOnly, List<AttendanceReportDto>> GetFiveSaturdaysAttendanceReportDicionary()
        {
            return new Dictionary<DateOnly, List<AttendanceReportDto>>
            {
                [new DateOnly(2025, 5, 3)] = new List<AttendanceReportDto>
                {
                    new() { Name = "B", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) },
                    new() { Name = "A", Time = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes( 9 * 60 + 15)) }
                },
                [new DateOnly(2025, 5, 10)] = new List<AttendanceReportDto>()
                {
                    new()  { Name = "C", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)) },
                    new()  { Name = "D", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(11)) }
                },
                [new DateOnly(2025, 5, 17)] = new List<AttendanceReportDto>{
                    new() { Name = "E", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(12)) },
                    new() { Name = "F", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(13)) }
                },
                [new DateOnly(2025, 5, 24)] = new List<AttendanceReportDto>
                {
                    new() { Name = "G", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)) },
                    new() { Name = "H", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(15)) }
                },
                [new DateOnly(2025, 5, 31)] = new List<AttendanceReportDto>
                {
                    new() { Name = "I", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(16)) },
                    new() { Name = "J", Time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)) }
                }
            };
        }

        private void AssertFiveSaturdaysAttendancesReport(ExcelPackage package)
        {
            var worksheets = package.Workbook.Worksheets;

            Assert.All(worksheets, w =>
            {
                Assert.Equal("Nome", w.Cells["B2"].GetValue<string>());
                Assert.Equal("Horário", w.Cells["C2"].GetValue<string>());
            });

            Assert.Equal("3", worksheets[0].Name);
            AssertWorksheetTwoRowsTable(worksheets[0], "A", "09:15", "B", "08:00");

            Assert.Equal("10", worksheets[1].Name);
            AssertWorksheetTwoRowsTable(worksheets[1], "C", "10:00", "D", "11:00");

            Assert.Equal("17", worksheets[2].Name);
            AssertWorksheetTwoRowsTable(worksheets[2], "E", "12:00", "F", "13:00");

            Assert.Equal("24", worksheets[3].Name);
            AssertWorksheetTwoRowsTable(worksheets[3], "G", "14:00", "H", "15:00");

            Assert.Equal("31", worksheets[4].Name);
            AssertWorksheetTwoRowsTable(worksheets[4], "I", "16:00", "J", "17:00");
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

        private void AssertWorksheetTwoRowsTable(ExcelWorksheet excelWorksheet, string b3, string c3, string b4, string c4)
        {
            Assert.Equal(b3, excelWorksheet.Cells["B3"].GetValue<string>());
            Assert.Equal(c3, excelWorksheet.Cells["C3"].GetValue<string>());
            Assert.Equal(b4, excelWorksheet.Cells["B4"].GetValue<string>());
            Assert.Equal(c4, excelWorksheet.Cells["C4"].GetValue<string>());
        }
    }
}
