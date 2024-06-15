
using System.Linq.Expressions;

namespace Piba.Services
{
    public class ExcelWorksheetData<T> where T : class
    {
        public ExcelWorksheetData(string name)
        {
            Name = name;
            Map = new();
            Rows = new();
        }

        public string Name { get; set; }
        public Dictionary<string, Func<T, object>> Map { get; set; }
        public List<T> Rows { get; set; }
    }
}