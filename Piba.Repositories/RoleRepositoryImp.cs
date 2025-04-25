using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class RoleRepositoryImp : RoleRepository
    {
        private readonly PibaDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepositoryImp(PibaDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task<int> GetTotalAsync()
        {
            var total = await _dbContext.Set<IdentityRole>().CountAsync();
            return total;
        }

        public async Task<List<IdentityRole>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var records = await _dbContext.Set<IdentityRole>()
                    .Skip(paginationQueryParameters.Skip)
                    .Take(paginationQueryParameters.Take)
                .ToListAsync();

            return records;
        }

        public async Task CreateAsync(string name)
        {
            await _roleManager.CreateAsync(new IdentityRole(name));
        }

        public async Task DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return;
            }

            await _roleManager.DeleteAsync(role);
        }

        public async Task<List<string>> GetRoleOptionsAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }
    }
}
