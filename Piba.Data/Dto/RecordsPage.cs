namespace Piba.Data.Dto
{
    public class RecordsPage<T>
    {
        public int Total { get; set; }
        public List<T> Records { get; set; }

    }
}
