using System.Text.Json.Serialization;

namespace Piba.Data.Entities
{
    public class ScheduleSong
    {
        public Guid Id { get; set; }

        public Guid ScheduleId { get; set; }

        [JsonIgnore]
        public Schedule Schedule { get; set; }

        public Guid SongId { get; set; }

        public Song Song { get; set; }
    }
}
