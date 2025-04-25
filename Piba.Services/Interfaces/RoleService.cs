using Microsoft.AspNetCore.Identity;
using Piba.Data.Dto;

namespace Piba.Services.Interfaces
{
    public interface RoleService
    {
        Task<RecordsPage<IdentityRole>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
        Task CreateAsync(string name);

        Task DeleteAsync(string id);
        Task<List<string>> GetRoleOptionsAsync();
    }
}
