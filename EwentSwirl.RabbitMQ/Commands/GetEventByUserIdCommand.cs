using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class GetEventsByUserIdCommand: Command
    {
        public int UserId { get; set; }
    }

    public class GetEventsByUserIdResponse: CommandResponse
    {
        public Event[] Events { get; set; }
    }
}
