namespace Piba.Data.Dto
{
    public class SongDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public List<LinkDto> Links { get; set; }
    }
}
