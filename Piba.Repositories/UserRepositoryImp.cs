using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class UserRepositoryImp : UserRepository
    {
        private readonly PibaDbContext _dbContext;
        private readonly UserManager<PibaUser> _userManager;

        public UserRepositoryImp(PibaDbContext dbContext, UserManager<PibaUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<UserOptionDto>> GetUserOptionsAsync()
        {
            var options = await _dbContext.Set<PibaUser>()
                .Select(u => new UserOptionDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                }).ToListAsync();

            return options;
        }

        public async Task<List<PibaUser>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var records = await _dbContext.Set<PibaUser>()
                    .Skip(paginationQueryParameters.Skip)
                    .Take(paginationQueryParameters.Take)
                .ToListAsync();

            return records;
        }

        public async Task<int> GetTotalAsync()
        {
            var total = await _dbContext.Set<PibaUser>().CountAsync();
            return total;
        }

        public async Task<PibaUser> GetByIdAsync(string id)
        {
            var user = await _dbContext.Set<PibaUser>()
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task UpdateAsync(PibaUser userEntity)
        {
            _dbContext.Set<PibaUser>().Update(userEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AssignRoleAsync(PibaUser userEntity, string role)
        {
            await _userManager.AddToRoleAsync(userEntity, role);
        }

        public async Task RemoveFromRoleAsync(PibaUser userEntity, string roleId)
        {
            await _userManager.RemoveFromRoleAsync(userEntity, roleId);
        }

        public async Task<List<string>> GetUserRolesAsync(PibaUser user)
        { 
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    }
}
