using EventSwirl.Domain.Enums;

namespace EventSwirl.Domain.Entities
{
    public class Event : DomainObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDateTime { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public AgeLimitation AgeLimitation { get; set; }

        public User Creator { get; set; }
    }
}
