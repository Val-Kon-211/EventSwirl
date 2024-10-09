namespace EwentSwirl.RabbitMQ.Commands
{
    public class DeleteEventByIdCommand: Command
    {
        public int EventId { get; set; }
    }

    public class DeleteEventByIdResponse: CommandResponse { }
}
