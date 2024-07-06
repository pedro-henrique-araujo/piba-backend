using OfficeOpenXml;

namespace Piba.Services.Tests
{
    public class CellWrapperTests : IDisposable
    {
        private readonly ExcelRange _cell;
        private readonly CellWrapper _cellWrapper;
        private readonly ExcelPackage _excelPackage;

        public CellWrapperTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelPackage = new ExcelPackage();
            var worksheet = _excelPackage.Workbook.Worksheets.Add("a");
            _cell = worksheet.Cells["A1"];
            _cellWrapper = new CellWrapper(_cell);
        }

        [Fact]
        public void SetBold_WhenCalled_SetBoldCorrectly()
        {
            _cellWrapper.SetBold();
            Assert.True(_cell.Style.Font.Bold);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData("abc")]
        [InlineData('t')]
        [InlineData(null)]
        public void SetValue_WhenCalled_SetValueCorrectly(object value)
        {
            _cellWrapper.SetValue(value);
            Assert.Equal(value, _cell.Value);
        }

        public void Dispose()
        {
            _excelPackage.Dispose();
        }
    }
}
