using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface UserRepository
    {
        Task<List<UserOptionDto>> GetUserOptionsAsync();

        Task<List<PibaUser>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);

        Task<int> GetTotalAsync();

        Task<PibaUser> GetByIdAsync(string id);
        Task UpdateAsync(PibaUser userEntity);
        Task AssignRoleAsync(PibaUser userEntity, string roleId);
        Task RemoveFromRoleAsync(PibaUser userEntity, string roleId);
        Task<List<string>> GetUserRolesAsync(PibaUser user);
    }
}