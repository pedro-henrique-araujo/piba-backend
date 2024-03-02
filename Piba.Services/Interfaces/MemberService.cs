using Piba.Data.Dto;

namespace Piba.Services.Interfaces
{
    public interface MemberService
    {
        Task<List<MemberOptionDto>> GetOptionsAsync();
        Task ReviewMembersActivityAsync();
    }
}