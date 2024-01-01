using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface MemberRepository
    {
        IQueryable<Member> GetAllInactiveAndActiveQueryable();
    }
}
