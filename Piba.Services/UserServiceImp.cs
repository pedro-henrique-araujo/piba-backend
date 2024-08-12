using Piba.Data.Dto;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class UserServiceImp : UserService
    {
        private readonly UserRepository _userRepository;

        public UserServiceImp(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserOptionDto>> GetUserOptionsAsync()
        {
            return await _userRepository.GetUserOptionsAsync();
        }
    }
}
