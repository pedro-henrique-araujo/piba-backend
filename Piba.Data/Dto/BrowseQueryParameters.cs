

namespace Piba.Data.Dto
{
    public class BrowseQueryParameters
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;

        public string? Search { get; set; }
    }
}
