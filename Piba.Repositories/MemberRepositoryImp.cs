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
        
        public async Task<List<MemberOptionDto>> GetAllInactiveAndActiveAsync()
        {
            return await _dbContext.Set<Member>()
                .Where(m => new[] { MemberStatus.Inactive, MemberStatus.Active }.Contains(m.Status))
                .Select(m => new MemberOptionDto { Id = m.Id, Name = m.Name })
                .ToListAsync();
        }
    }
}
