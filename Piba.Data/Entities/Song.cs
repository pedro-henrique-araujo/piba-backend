namespace Piba.Data.Entities
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Link> Links { get; set; }
    }
}
