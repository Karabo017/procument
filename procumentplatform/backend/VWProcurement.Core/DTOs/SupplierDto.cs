using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.DTOs
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? CompanyRegistrationNumber { get; set; }
        public string? Address { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateSupplierDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? CompanyRegistrationNumber { get; set; }
        public string? Address { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateSupplierDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? CompanyRegistrationNumber { get; set; }
        public string? Address { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
