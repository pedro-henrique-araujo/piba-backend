using Piba.Data.Entities;

namespace Piba.Data.Dto
{
    public class UserDto
    {
        public UserDto(PibaUser? user)
        {
            Id = user.Id;
            Name = user.Name;
            PhotoUrl = user.PhotoUrl;
        }

        public string Id { get; private set; }
        public string? Name { get; private set; }
        public string? PhotoUrl { get; private set; }
    }
}