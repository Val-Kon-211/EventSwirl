namespace EwentSwirl.RabbitMQ
{
    public abstract class CommandResponse
    {
        public Guid CommandId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
