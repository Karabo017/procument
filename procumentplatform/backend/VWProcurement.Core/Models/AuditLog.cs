using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.Models
{
    public enum AuditAction
    {
        Create = 0,
        Update = 1,
        Delete = 2,
        Login = 3,
        Logout = 4,
        View = 5,
        Download = 6,
        Upload = 7,
        Publish = 8,
        Award = 9,
        Submit = 10,
        Approve = 11,
        Reject = 12
    }
    
    public class AuditLog
    {
        public Guid Id { get; set; }
        
        public Guid? UserId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string EntityType { get; set; } = string.Empty; // "Tender", "Bid", "User", etc.
        
        public Guid? EntityId { get; set; }
        
        [Required]
        public AuditAction Action { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(2000)]
        public string? OldValues { get; set; } // JSON of old values
        
        [StringLength(2000)]
        public string? NewValues { get; set; } // JSON of new values
        
        [StringLength(50)]
        public string? IpAddress { get; set; }
        
        [StringLength(500)]
        public string? UserAgent { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public Guid? TenantId { get; set; } // For multi-tenancy
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}