namespace EwentSwirl.RabbitMQ
{
    public interface IProducer
    {
        Task<TResponse> SendCommandAsync<TCommand, TResponse>(TCommand command) where TCommand : Command where TResponse : CommandResponse;
    }
}
