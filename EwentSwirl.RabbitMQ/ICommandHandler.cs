namespace EwentSwirl.RabbitMQ
{
    public interface ICommandHandler
    {
        Task HandleAsync(Command command);

        bool CanHandle(Command command);
    }
}
