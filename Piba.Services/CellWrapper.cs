using OfficeOpenXml;

namespace Piba.Services
{
    public class CellWrapper
    {
        private readonly ExcelRange _excelRange;

        public CellWrapper(ExcelRange excelRange)
        {
            _excelRange = excelRange;
        }

        public void SetBold()
        {
            _excelRange.Style.Font.Bold = true;
        }

        public void SetValue(object value)
        {
            _excelRange.Value = value;
        }
    }
}