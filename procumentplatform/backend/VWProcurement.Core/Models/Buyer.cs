using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.Models
{
    public class Buyer
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(255)]
        public string? Department { get; set; }
        
        [StringLength(50)]
        public string? EmployeeId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [Required]
        [StringLength(255)]
        public string OrganizationName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string OrganizationType { get; set; } = string.Empty; // Government, Private, SOE, NGO
        
        [StringLength(255)]
        public string? DepartmentName { get; set; }
        
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
        public string? TaxNumber { get; set; }
        
        [Required]
        [StringLength(255)]
        public string AuthorizedSignatory { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string ProcurementAuthority { get; set; } = string.Empty;
        
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerificationDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Tender> Tenders { get; set; } = new List<Tender>();
    }
}
