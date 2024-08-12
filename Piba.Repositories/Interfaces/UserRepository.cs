using Piba.Data.Dto;

namespace Piba.Repositories.Interfaces
{
    public interface UserRepository
    {
        Task<List<UserOptionDto>> GetUserOptionsAsync();
    }
}
