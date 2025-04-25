using Microsoft.AspNetCore.Identity;
using Piba.Data.Dto;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class RoleServiceImp : RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleServiceImp(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RecordsPage<IdentityRole>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var page = new RecordsPage<IdentityRole>();
            page.Records = await _roleRepository.PaginateAsync(paginationQueryParameters);
            page.Total = await _roleRepository.GetTotalAsync();
            return page;
        }

        public async Task CreateAsync(string name)
        {
            await _roleRepository.CreateAsync(name);
        }

        public async Task DeleteAsync(string id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        public async Task<List<string>> GetRoleOptionsAsync()
        {
            return await _roleRepository.GetRoleOptionsAsync();
        }
    }
}
