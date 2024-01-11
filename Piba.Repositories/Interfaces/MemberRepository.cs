using Piba.Data.Dto;

namespace Piba.Repositories.Interfaces
{
    public interface MemberRepository
    {
        Task<List<MemberOptionDto>> GetAllInactiveAndActiveAsync();
    }
}
