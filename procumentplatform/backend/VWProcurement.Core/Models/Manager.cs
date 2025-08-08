using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.Models
{
    public class Manager
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string Name => $"{FirstName} {LastName}";
        
        [Required]
        [StringLength(100)]
        public string Role { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Department { get; set; }
        
        [StringLength(50)]
        public string AccessLevel { get; set; } = "Standard";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
}
