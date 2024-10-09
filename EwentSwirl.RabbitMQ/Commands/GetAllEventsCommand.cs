using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class GetAllEventsCommand: Command {}

    public class GetAllEventsResponse : CommandResponse
    {
        public Event[] Events { get; set; }
    }
}
