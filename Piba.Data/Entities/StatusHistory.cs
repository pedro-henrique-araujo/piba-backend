namespace Piba.Data.Entities
{
    public class StatusHistory
    {
        public Guid Id { get; set; }
        public bool IsSent { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public StatusHistory()
        {
            var today = DateTime.UtcNow;
            var aMonthAgo = today.AddMonths(-1);
            Month = aMonthAgo.Month;
            Year = aMonthAgo.Year;
        }
    }
}
