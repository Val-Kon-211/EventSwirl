using System.ComponentModel.DataAnnotations;

namespace EventSwirl.Domain.Entities
{
    public class DomainObject
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
