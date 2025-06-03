using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Piba.Data.Dto
{
    public class AttachmentDto
    {
        public string Name { get; set; }
        public byte[]? Bytes { get; set; }

        public AttachmentDto()
        {
            Name = string.Empty;
            Bytes = Array.Empty<byte>();
        }

        public AttachmentDto(string name, byte[]? bytes)
        {
            Name = name;
            Bytes = bytes;
        }
    }
}
