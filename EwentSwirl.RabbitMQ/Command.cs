namespace EwentSwirl.RabbitMQ
{
    public abstract class Command
    {
        public Guid CommandId { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
