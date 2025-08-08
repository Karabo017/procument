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
        public string? Name { get; set; }
        
        [StringLength(255)]
        public string? CompanyName { get; set; }
        
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string UserType { get; set; } = string.Empty; // "Supplier", "Buyer", "Manager"
        
        public bool IsActive { get; set; } = true;
        
        public bool EmailVerified { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        [StringLength(500)]
        public string? ProfilePhotoPath { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
