using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
