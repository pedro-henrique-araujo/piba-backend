using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class UserRepositoryImp : UserRepository
    {
        private readonly PibaDbContext _dbContext;

        public UserRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserOptionDto>> GetUserOptionsAsync()
        {
            var options = await _dbContext.Set<IdentityUser>()
                .Select(u => new UserOptionDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                }).ToListAsync();

            return options;
        }
    }
}
