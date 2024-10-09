using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class DeleteUserCommand: Command
    {
        public User User { get; set; }
    }

    public class DeleteUserResponse : CommandResponse { }
}
