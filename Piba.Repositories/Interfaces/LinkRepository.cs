using Piba.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piba.Repositories.Interfaces
{
    public interface LinkRepository
    {
        Task CreateRangeAsync(List<Link> linksToSave);
        Task DeleteRangeAsync(List<Link> linksToDelete);
        Task<List<Link>> GetBySongIdAsync(Guid songId);
        Task UpdateRangeAsync(List<Link> links);
    }
}
