using OfficeOpenXml;

namespace Piba.Services
{
    public class ExcelWrapper : IDisposable
    {
        private readonly ExcelPackage _excelPackage;

        public ExcelWrapper()
        {
            _excelPackage = new ExcelPackage();
        }

        public async Task<byte[]> GetByteArrayAsync()
        {
            return await _excelPackage.GetAsByteArrayAsync();
        }

        public void AddWorksheet<T>(ExcelWorksheetData<T> data) where T : class
        {
            var worksheetWrapper = new WorksheetWrapper(_excelPackage.Workbook.Worksheets.Add(data.Name));
            ForeachIndex(data.Map, (keyValuePair, index) =>
            {
                var (columnName, getValue) = keyValuePair;
                worksheetWrapper.AddTitle(index, columnName);

                ForeachIndex(
                    data.Rows,
                    (row, internalIndex) =>
                    {
                        var cell = worksheetWrapper.At(internalIndex, index);
                        var value = getValue(row);
                        cell.SetValue(value);
                    }
                );
            });

            worksheetWrapper.AutoFitColumns();
        }

        private void ForeachIndex<T>(IEnumerable<T> enumerable, Action<T, int> action)
        {
            for (int i = 0; i < enumerable.Count(); i++)
            {
                var value = enumerable.ElementAt(i);
                action(value, i);
            }
        }

        public void Dispose()
        {
            _excelPackage.Dispose();
        }
    }
}
