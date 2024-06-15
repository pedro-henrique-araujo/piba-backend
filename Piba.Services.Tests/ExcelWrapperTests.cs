using OfficeOpenXml;

namespace Piba.Services.Tests
{
    public class ExcelWrapperTests
    {
        private readonly List<ExcelWorksheet> _worksheets;
        private readonly string _dateFormat;

        public ExcelWrapperTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _worksheets = new List<ExcelWorksheet>();
            _dateFormat = "yyyy-MM-dd HH:mm:ss";
        }

        [Fact]
        public async Task AddWorksheet_WhenMultipleWorsheetsAreCreated_MatchNumberOfWorksheets()
        {
            using var excelWrapper = new ExcelWrapper();
            excelWrapper.AddWorksheet<TestValues>(new(name: "a"));
            excelWrapper.AddWorksheet<TestValues>(new(name: "b"));
            excelWrapper.AddWorksheet<TestValues>(new(name: "c"));
            using var package = LoadPackage(await excelWrapper.GetByteArrayAsync());
            Assert.Equal(3, package.Workbook.Worksheets.Count);

        }

        [Fact]
        public async Task AddWorksheet_WhenCalled_MatchData()
        {
            using var excelWrapper = new ExcelWrapper();
            var date = DateTime.UtcNow;
            excelWrapper.AddWorksheet<TestValues>(new(name: "abc")
            {
                Map = new()
                {
                    ["a"] = v => v.Prop1,
                    ["b"] = v => v.Prop2,
                    ["c"] = v => v.Prop3,
                    ["d"] = v => v.Prop4
                },
                Rows = new()
                {
                    new() { Prop1 = 1, Prop2 = "d", Prop3 = date },
                    new() { Prop1 = 2, Prop2 = "e", Prop3 = date.AddDays(1), Prop4 = 3 },
                }
            });

            using var package = LoadPackage(await excelWrapper.GetByteArrayAsync());
            _worksheets.Add(package.Workbook.Worksheets.First());
            var worksheet = _worksheets.First();

            Assert.Equal("abc", _worksheets.First().Name);
            AssertCorrectTitleRow();
            AssertCorrectBodyRows(date);
            AssertRowsAreNotBold();
            Assert.NotEqual(worksheet.Columns[3].Width, worksheet.Columns[4].Width);
        }

        private void AssertCorrectBodyRows(DateTime date)
        {
            var cells = _worksheets.First().Cells;

            Assert.Equal(1, cells["B3"].GetValue<int>());

            Assert.Equal("d", cells["C3"].GetValue<string>());

            Assert.Equal(date.ToString(_dateFormat), cells["D3"].GetValue<DateTime>().ToString(_dateFormat));

            Assert.Null(cells["E3"].Value);

            Assert.Equal(2, cells["B4"].GetValue<int>());

            Assert.Equal("e", cells["C4"].GetValue<string>());

            Assert.Equal(date.AddDays(1).ToString(_dateFormat), cells["D4"].GetValue<DateTime>().ToString(_dateFormat));

            Assert.Equal(3, cells["E4"].GetValue<int>());
        }

        private void AssertRowsAreNotBold()
        {
            var cells = _worksheets.First().Cells;

            Assert.False(cells["B3:E4"].Style.Font.Bold);
        }

        private void AssertCorrectTitleRow()
        {
            var cells = _worksheets.First().Cells;

            Assert.Equal("a", cells["B2"].Value);

            Assert.Equal("b", cells["C2"].Value);

            Assert.Equal("c", cells["D2"].Value);

            Assert.Equal("d", cells["E2"].Value);

            Assert.True(cells["B2:E2"].Style.Font.Bold);
        }

        private ExcelPackage LoadPackage(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            return new ExcelPackage(stream);
        }
    }

    public class TestValues
    {
        public int Prop1 { get; set; }
        public string Prop2 { get; set; }
        public DateTime Prop3 { get; set; }
        public int? Prop4 { get; internal set; }
    }
}
