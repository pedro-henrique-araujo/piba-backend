using Piba.Data.Enums;

namespace Piba.Data.Entities
{
    public class Link
    {
        public Guid Id { get; set; }
        public LinkSource Source { get; set; }
        public bool IsMain { get; set; }

        public string Url { get; set; }

        public Guid SongId { get; set; }
        public Song Song { get; set; }

    }
}
