using Microsoft.AspNetCore.Identity;
using Piba.Data.Entities;

namespace Piba.Repositories
{
    public interface AuthorizationRepository
    {
        Task<IList<string>> GetUserRolesAsync(PibaUser user);
    }
}