using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class GetEventByIdCommand: Command
    {
        public int EventId { get; set; }
    }

    public class GetEventByIdResponse: CommandResponse
    {
        public Event Event { get; set; }
    }
}
