namespace EventSwirl.Domain.Entities
{
    public class UserEvent : DomainObject
    {
        public int EventId { get; set; }

        public int UserId { get; set; }
    }
}
