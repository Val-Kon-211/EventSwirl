using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class CreateEventCommand: Command
    {
        public Event Event { get; set; }
    }

    public class CreateEventResponse: CommandResponse { }
}
