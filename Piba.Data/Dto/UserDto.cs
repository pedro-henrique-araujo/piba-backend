using Piba.Data.Entities;

namespace Piba.Data.Dto
{
    public class UserDto
    {
        public UserDto()
        {
            
        }

        public UserDto(PibaUser? user)
        {
            Id = user.Id;
            Name = user.Name;
            PhotoUrl = user.PhotoUrl;
        }

        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public List<string> Roles { get; set; }
    }
}