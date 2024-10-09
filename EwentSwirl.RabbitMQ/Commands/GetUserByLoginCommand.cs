using EventSwirl.Domain.Entities;

namespace EwentSwirl.RabbitMQ.Commands
{
    public class GetUserByLoginCommand: Command
    {
        public string Login { get; set; }
    }

    public class GetUserByLoginResponse : CommandResponse
    {
        public User User { get; set; }
    }
}
