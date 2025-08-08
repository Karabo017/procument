namespace VWProcurement.Core.Models
{
    public class SupplierCompliance
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public Guid RequirementId { get; set; }
        public string Status { get; set; } = "Pending";
        public string? DocumentPath { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid? VerifiedBy { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public Supplier Supplier { get; set; } = null!;
        public ComplianceRequirement Requirement { get; set; } = null!;
        public User? VerifiedByUser { get; set; }
    }
}