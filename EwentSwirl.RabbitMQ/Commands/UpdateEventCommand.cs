using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class UpdateEventCommand: Command
    {
        public Event Event { get; set; }
    }

    public class UpdateEventResponse: CommandResponse { }
}
