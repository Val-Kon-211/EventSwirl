using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;


namespace EwentSwirl.RabbitMQ
{
    public abstract class BaseCommandHandler<TCommand, TCommandResponse> : ICommandHandler where TCommand : Command where TCommandResponse : CommandResponse
    {
        protected readonly IModel _channel;

        public BaseCommandHandler()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public bool CanHandle(Command command) => command is TCommand;

        public async Task HandleAsync(Command command)
        {
            if (command is TCommand typedCommand)
            {
                Console.WriteLine($"[rabbit info]: Handle command with CommandId = {command.CommandId}");
                var response = await HandleCommandAsync(typedCommand).ConfigureAwait(false);

                // Отправляем ответ в очередь
                var responseMessage = JsonConvert.SerializeObject(response);
                var responseBody = Encoding.UTF8.GetBytes(responseMessage);

                _channel.BasicPublish(exchange: "", routingKey: "responses_queue", body: responseBody);
                Console.WriteLine($"[rabbit info]: Receive {typeof(TCommandResponse)}");
            }
        }

        protected abstract Task<TCommandResponse> HandleCommandAsync(TCommand command);
    }

}
