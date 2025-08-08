using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VWProcurement.Core.Models
{
    public enum BidStatus
    {
        Submitted,
        UnderReview,
        Shortlisted,
        Awarded,
        Rejected
    }

    public class Bid
    {
        public Guid Id { get; set; }
        
        [StringLength(100)]
        public string BidNumber { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal BidAmount { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        public string? Proposal { get; set; }
        
        public string? AttachmentPath { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Submitted";
        
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? SubmittedAt { get; set; }
        
        public int ValidityPeriod { get; set; } = 90;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string? Notes { get; set; }
        
        public DateTime? ReviewedAt { get; set; }
        
        // Foreign Keys
        public Guid SupplierId { get; set; }
        
        public Guid TenderId { get; set; }
        
        // Navigation Properties
        public virtual Supplier Supplier { get; set; } = null!;
        
        public virtual Tender Tender { get; set; } = null!;
    }
}
