using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Concurrent;

namespace EwentSwirl.RabbitMQ
{
    public class Producer: IProducer
    {
        private readonly IModel _channel;

        public Producer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public async Task<TResponse> SendCommandAsync<TCommand, TResponse>(TCommand command) where TCommand : Command where TResponse : CommandResponse
        {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            // Формируем сообщение с указанием имени команды и сериализованных данных
            var message = new
            {
                CommandName = command.GetType().Name,
                Body = JsonConvert.SerializeObject(command)
            };

            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(exchange: "", routingKey: "commands_queue", basicProperties: properties, body: messageBody);
            Console.WriteLine($"[rabbit info]: Send {message.CommandName} with CommandId = {command.CommandId}");


            // Ждём ответ асинхронно
            return await WaitForResponseAsync<TResponse>(command.CommandId);
        }

        private async Task<TResponse> WaitForResponseAsync<TResponse>(Guid commandId) where TResponse: CommandResponse
        {
            Console.WriteLine($"[rabbit info]: Waiting for response from command with CommandId = {commandId}");
            // Логика ожидания ответа
            var tcs = new TaskCompletionSource<TResponse>();

            var responseConsumer = new EventingBasicConsumer(_channel);
            responseConsumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var response = JsonConvert.DeserializeObject<TResponse>(message);

                if (response != null && response.CommandId == commandId)
                {
                    tcs.SetResult(response);
                }
            };

            _channel.BasicConsume(queue: "responses_queue", autoAck: true, consumer: responseConsumer);

            return await tcs.Task;
        }
    }
}