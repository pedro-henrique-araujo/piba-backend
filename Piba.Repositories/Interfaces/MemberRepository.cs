using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface MemberRepository
    {
        Task<List<Member>> GetAllActiveCreatedBefore21DaysAgoAsync();
        Task<List<Member>> GetAllInactiveAndActiveAsync();
        Task<List<MemberOptionDto>> GetAllOptionsAsync();
        Task<List<MemberOptionDto>> GetAllInactiveAndActiveOptionsAsync();
        Task<List<Member>> GetAllInactiveAsync();
        Task<List<Member>> GetAllAlwaysExcusedAsync();
        Task SaveChangesAsync();
    }
}
