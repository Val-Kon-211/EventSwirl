using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class CreateUserCommand : Command
    {
        public User User { get; set; }
    }

    public class CreateUserResponse : CommandResponse { }
}
