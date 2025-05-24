using Piba.Data.Enums;

namespace Piba.Data.Dto
{
    public class LinkDto
    {
        public Guid? Id { get; set; }
        public LinkSource Source { get; set; }
        public bool IsMain { get; set; }

        public string Url { get; set; }

    }
}
