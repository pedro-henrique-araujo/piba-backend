using Piba.Data.Entities;

namespace Piba.Data.Dto
{
    public class MediaAvailabilityDto
    {
        public MediaAvailabilityDto(MediaAvailability mediaAvailability)
        {
            Id = mediaAvailability.Id;
            Date = mediaAvailability.Date;
            User = new UserDto(mediaAvailability.User);
        }

        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public UserDto User { get; private set; }
    }
}
