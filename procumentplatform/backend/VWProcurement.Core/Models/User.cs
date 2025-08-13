using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? FirstName { get; set; }
        
        [StringLength(255)]
        public string? LastName { get; set; }
        
        [StringLength(255)]
        public string? CompanyName { get; set; }
        
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; } = UserRole.PublicViewer;
        
        public UserStatus Status { get; set; } = UserStatus.Pending;
        
        public bool EmailVerified { get; set; } = false;
        
        public bool TwoFactorEnabled { get; set; } = false;
        
        [StringLength(10)]
        public string? TwoFactorCode { get; set; }
        
        public DateTime? TwoFactorCodeExpiry { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        [StringLength(500)]
        public string? ProfilePhotoPath { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(50)]
        public string? CSDNumber { get; set; } // Central Supplier Database number
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(10)]
        public string? PostalCode { get; set; }
        
        public decimal? AnnualTurnover { get; set; }
        
        [StringLength(1000)]
        public string? BusinessDescription { get; set; }
        
        public Guid? TenantId { get; set; } // For multi-tenancy
        
        // Navigation properties
        public virtual ICollection<Tender> CreatedTenders { get; set; } = new List<Tender>();
        public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public virtual ICollection<TenderQuestion> Questions { get; set; } = new List<TenderQuestion>();
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
