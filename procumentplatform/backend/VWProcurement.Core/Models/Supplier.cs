using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        [StringLength(100)]
        public string? CompanyRegistrationNumber { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string BusinessRegistrationNumber { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? TaxNumber { get; set; }
        
        [Required]
        [StringLength(255)]
        public string ContactPerson { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string BusinessAddress { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Province { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Country { get; set; } = "South Africa";
        
        [StringLength(100)]
        public string? BusinessType { get; set; }
        
        public int? YearsInBusiness { get; set; }
        
        public int? NumberOfEmployees { get; set; }
        
        public decimal? AnnualTurnover { get; set; }
        
        [StringLength(255)]
        public string? Website { get; set; }
        
        [StringLength(50)]
        public string? BEELevel { get; set; }
        
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerificationDate { get; set; }
        
        [StringLength(50)]
        public string ComplianceStatus { get; set; } = "Pending";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
