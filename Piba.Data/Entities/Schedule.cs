namespace Piba.Data.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }

        public List<ScheduleSong> ScheduleSongs { get; set; }
    }
}
