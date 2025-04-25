using Microsoft.AspNetCore.Identity;
using Piba.Data.Dto;

namespace Piba.Repositories.Interfaces
{
    public interface RoleRepository
    {
        Task<int> GetTotalAsync();
        Task<List<IdentityRole>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
        Task CreateAsync(string name);

        Task DeleteAsync(string id);

        Task<List<string>> GetRoleOptionsAsync();
    }
}
