using OfficeOpenXml;

namespace Piba.Services.Tests
{
    public class WorksheetWrapperTests : IDisposable
    {
        private readonly ExcelPackage _excelPackage;
        private readonly ExcelWorksheet _worksheet;
        private readonly ExcelRange _cells;
        private readonly WorksheetWrapper _worksheetWrapper;

        public WorksheetWrapperTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelPackage = new ExcelPackage();
            _worksheet = _excelPackage.Workbook.Worksheets.Add("a");
            _cells = _worksheet.Cells;
            _worksheetWrapper = new WorksheetWrapper(_worksheet);
        }

        [Fact]
        public void At_WhenCalled_ReturnCorrectCell()
        {
            _worksheetWrapper.At(0, 0).SetValue("abc");
            Assert.Equal("abc", _cells["B3"].GetValue<string>());
        }

        [Fact]
        public void AddTitle_WhenCalled_FillCellValueCorrectly()
        {
            _worksheetWrapper.AddTitle(0, "abc");
            var cell = _cells["B2"];
            Assert.Equal("abc", cell.GetValue<string>());
            Assert.True(cell.Style.Font.Bold);
        }

        [Fact]
        public void AutoFitColumns_WhenWorksheetHasValue_AutoFitCorrectly()
        {
            _worksheetWrapper.At(0, 0).SetValue(Guid.NewGuid());
            _worksheetWrapper.AutoFitColumns();
            Assert.NotEqual(_worksheet.Columns[1].Width, _worksheet.Columns[2].Width);
        }

        [Fact]
        public void AutoFitColumn_WhenWorksheetIsBlank_DontThrowException()
        {
            _worksheetWrapper.AutoFitColumns();
            Assert.True(true);
        }

        public void Dispose()
        {
            _excelPackage.Dispose();
        }
    }
}
