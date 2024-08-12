using Piba.Data.Dto;

namespace Piba.Services.Interfaces
{
    public interface UserService
    {
        Task<List<UserOptionDto>> GetUserOptionsAsync();
    }
}
