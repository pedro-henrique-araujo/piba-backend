using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface UserService
    {
        Task<List<UserOptionDto>> GetUserOptionsAsync();

        Task<RecordsPage<PibaUser>> PaginateAsync(PaginationQueryParameters paginationQueryParameters);

        Task<UserDto> GetByIdAsync(string id);
        Task UpdateAsync(string id, UserDto userUpdateDto);
    }
}
