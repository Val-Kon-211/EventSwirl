using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using EwentSwirl.RabbitMQ.Commands;

namespace EwentSwirl.RabbitMQ
{
    public class CommandDispatcher: ICommandDispatcher
    {
        private readonly IModel _channel;
        private readonly IEnumerable<ICommandHandler> _handlers;
        private readonly IDictionary<string, Type> _commandTypeMap = new Dictionary<string, Type>
                    {
                        { "CreateEventCommand", typeof(CreateEventCommand) },
                        { "CreateUserCommand", typeof(CreateUserCommand) },
                        { "DeleteEventByIdCommand", typeof(DeleteEventByIdCommand) },
                        { "DeleteUserCommand", typeof(DeleteUserCommand) },
                        { "GetAllEventsCommand", typeof(GetAllEventsCommand) },
                        { "GetEventByIdCommand", typeof(GetEventByIdCommand) },
                        { "GetEventsByUserIdCommand", typeof(GetEventsByUserIdCommand) },
                        { "GetUserByIdCommand", typeof(GetUserByIdCommand) },
                        { "GetUserByLoginCommand", typeof(GetUserByLoginCommand) },
                        { "UpdateEventCommand", typeof(UpdateEventCommand) },
                        { "UpdateUserCommand", typeof(UpdateUserCommand) }
                    };

        public CommandDispatcher(IEnumerable<ICommandHandler> handlers)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _handlers = handlers;

            _channel.QueueDeclare(queue: "commands_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "responses_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public void StartListening()
        {
            var commandConsumer = new EventingBasicConsumer(_channel);
            commandConsumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    // Десериализуем сообщение для извлечения имени команды и её данных
                    var messageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                    if (messageData != null && messageData.ContainsKey("CommandName") && messageData.ContainsKey("Body"))
                    {
                        var commandName = messageData["CommandName"];
                        var commandBody = messageData["Body"];

                        // Определяем тип команды на основе имени
                        if (_commandTypeMap.TryGetValue(commandName, out var commandType))
                        {
                            var command = (Command)JsonConvert.DeserializeObject(commandBody, commandType);
                            var handler = _handlers.FirstOrDefault(h => h.CanHandle(command));

                            if (handler != null)
                            {
                                await handler.HandleAsync(command);
                            }
                            else
                            {
                                Console.WriteLine($"No handler found for command type: {command.GetType().Name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Unknown command type: {commandName}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing command: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "commands_queue", autoAck: true, consumer: commandConsumer);
            Console.WriteLine($"Dispatcher is listening on queue: commands_queue");
        }
    }
}
