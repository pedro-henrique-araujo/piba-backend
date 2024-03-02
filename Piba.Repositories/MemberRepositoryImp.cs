using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class MemberRepositoryImp : MemberRepository
    {
        private PibaDbContext _dbContext;

        public MemberRepositoryImp(PibaDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<MemberOptionDto>> GetAllInactiveAndActiveOptionsAsync()
        {
            return await _dbContext.Set<Member>()
                .Where(m => new[] { MemberStatus.Inactive, MemberStatus.Active }.Contains(m.Status))
                .Select(m => new MemberOptionDto { Id = m.Id, Name = m.Name })
                .ToListAsync();
        }

        public async Task<List<Member>> GetAllActiveAsync()
        {
            return await _dbContext.Set<Member>()
                .Where(m => m.Status == MemberStatus.Active)
                .ToListAsync();
        }

        public async Task<List<Member>> GetAllInactiveAsync()
        {
            return await _dbContext.Set<Member>()
               .Where(m => m.Status == MemberStatus.Inactive)
               .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
