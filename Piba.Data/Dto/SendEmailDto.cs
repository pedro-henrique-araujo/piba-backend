using System.Net.Mime;

namespace Piba.Data.Dto
{
    public class SendEmailDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }
    }
}
