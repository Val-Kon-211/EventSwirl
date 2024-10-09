using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class GetUserByIdCommand : Command
    {
        public int UserId { get; set; }
    }

    public class GetUserByIdResponse : CommandResponse
    {
        public User User { get; set; }
    }
}
