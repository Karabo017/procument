using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.DTOs
{
    public class BuyerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Department { get; set; }
        public string? EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBuyerDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Department { get; set; }
        public string? EmployeeId { get; set; }
    }

    public class UpdateBuyerDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Department { get; set; }
        public string? EmployeeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
