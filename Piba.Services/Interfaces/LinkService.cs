using Piba.Data.Dto;
using Piba.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piba.Services.Interfaces
{
    public interface LinkService
    {
        Task CreateLinksAsync(Guid id, List<LinkDto> links);
        Task UpdateLinksAsync(Guid id, List<LinkDto> links);
    }
}
