using EventSwirl.Domain.Entities;
using EventSwirl.Domain.Enums;

namespace EventSwirl.Application.Data.DTOs
{
    public class EventDTO
    {
        public int EventId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDateTime { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public AgeLimitation AgeLimitation { get; set; }

        public UserDTO Creator { get; set; }
    }
}
