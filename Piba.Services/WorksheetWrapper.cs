using OfficeOpenXml;

namespace Piba.Services
{
    public class WorksheetWrapper
    {
        private readonly ExcelWorksheet _worksheet;
        private readonly ExcelRange _cells;
        private readonly int _rowOffset;
        private readonly int _columnOffset;

        public WorksheetWrapper(ExcelWorksheet worksheet)
        {
            _worksheet = worksheet;
            _cells = _worksheet.Cells;
            _rowOffset = 3;
            _columnOffset = 2;
        }

        public CellWrapper At(int row, int column)
        {
            return new CellWrapper(_cells[row + _rowOffset, column + _columnOffset]);
        }

        public void AddTitle(int column, string name)
        {
            var cell = At(row: -1, column: column);
            cell.SetValue(name);
            cell.SetBold();
        }

        public void AutoFitColumns()
        {
            if (_worksheet.Dimension is null) return;
            _worksheet.Cells[_worksheet.Dimension.Address].AutoFitColumns();
        }
    }
}