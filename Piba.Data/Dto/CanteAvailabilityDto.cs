using Piba.Data.Entities;

namespace Piba.Data.Dto
{
    public class CanteAvailabilityDto
    {
        public CanteAvailabilityDto(CanteAvailability canteAvailability)
        {
            Id = canteAvailability.Id;
            Date = canteAvailability.Date;
            User = new UserDto(canteAvailability.User);
        }

        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public UserDto User { get; private set; }
    }
}
