using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface MemberService
    {
        Task<List<MemberOptionDto>> GetOptionsAsync();
        Task ReviewMembersActivityAsync();

        Task<RecordsPage<Member>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);
    }
}