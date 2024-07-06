using OfficeOpenXml;

namespace Piba.Services.Tests
{
    public class Common
    {
        public static ExcelPackage LoadExcelPackage(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            return new ExcelPackage(stream);
        }
    }
}
