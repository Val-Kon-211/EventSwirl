using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class UpdateUserCommand: Command
    {
        public User User { get; set; }
    }

    public class UpdateUserResponse: CommandResponse { }
}
