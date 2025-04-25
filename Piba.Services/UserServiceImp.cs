using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class UserServiceImp : UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        public UserServiceImp(UserRepository userRepository, RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<RecordsPage<PibaUser>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var page = new RecordsPage<PibaUser>();
            page.Records = await _userRepository.PaginateAsync(paginationQueryParameters);
            page.Total = await _userRepository.GetTotalAsync();
            return page;
        }

        public async Task<List<UserOptionDto>> GetUserOptionsAsync()
        {
            return await _userRepository.GetUserOptionsAsync();
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var roles = await _userRepository.GetUserRolesAsync(user);

            return new(user)
            {
                Roles = roles
            };
        }

        public async Task UpdateAsync(string id, UserDto user)
        {
            var userEntity = await _userRepository.GetByIdAsync(id);
            if (userEntity == null)
            {
                throw new Exception("User not found");
            }

            userEntity.Name = user.Name;
            userEntity.PhotoUrl = user.PhotoUrl;

            await _userRepository.UpdateAsync(userEntity);

            var rolesInDb = await _userRepository.GetUserRolesAsync(userEntity);
            var rolesToAdd = user.Roles.Except(rolesInDb).ToList();

            var rolesToRemove = rolesInDb.Except(user.Roles).ToList();

            foreach (var roleId in rolesToAdd)
            {
                await _userRepository.AssignRoleAsync(userEntity, roleId);
            }

            foreach (var roleId in rolesToRemove)
            {
                await _userRepository.RemoveFromRoleAsync(userEntity, roleId);
            }
        }
    }
}
