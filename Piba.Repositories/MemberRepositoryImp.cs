using Piba.Data;
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

        public IQueryable<Member> GetAllInactiveAndActiveQueryable()
        {
            return _dbContext.Set<Member>()
                .Where(m => new[] { MemberStatus.Inactive, MemberStatus.Active }.Contains(m.Status));
        }
    }
}
