using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VWProcurement.Core.Models
{
    public enum TenderStatus
    {
        Draft = 0,
        PendingApproval = 1,
        Open = 2,
        Closed = 3,
        Evaluation = 4,
        Awarded = 5,
        Cancelled = 6
    }

    public class Tender
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenderNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string? Requirements { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EstimatedValue { get; set; }
        
        public TenderStatus Status { get; set; } = TenderStatus.Draft;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? PublishDate { get; set; }
        
        public DateTime ClosingDate { get; set; }
        
        public DateTime? AwardDate { get; set; }
        
        public DateTime? PublishedAt { get; set; }
        
        public Guid? ApprovedByManagerId { get; set; }
        
        // New fields for enhanced functionality
        [StringLength(1000)]
        public string? BriefingLocation { get; set; }
        
        public DateTime? BriefingDate { get; set; }
        
        public bool IsPublic { get; set; } = true; // Public visibility
        
        public bool IsGovernmentIntegrated { get; set; } = false; // Synced with gov platform
        
        [StringLength(500)]
        public string? DocumentPath { get; set; } // Path to tender documents
        
        [StringLength(2000)]
        public string? EvaluationCriteria { get; set; }
        
        public decimal? MinimumScore { get; set; } = 70.0m;
        
        public bool AllowQuestions { get; set; } = true;
        
        public DateTime? QuestionDeadline { get; set; }
        
        [StringLength(1000)]
        public string? AwardJustification { get; set; }
        
        public Guid? AwardedToSupplierId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AwardedValue { get; set; }
        
        public Guid? TenantId { get; set; } // For multi-tenancy
        
        // Foreign Keys
        public Guid BuyerId { get; set; }
        public Guid CategoryId { get; set; }
        
        // Navigation Properties
        public virtual User Buyer { get; set; } = null!;
        public virtual TenderCategory Category { get; set; } = null!;
        public virtual User? ApprovedByManager { get; set; }
        public virtual User? AwardedToSupplier { get; set; }
        public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public virtual ICollection<TenderQuestion> Questions { get; set; } = new List<TenderQuestion>();
        public virtual ICollection<TenderDocument> Documents { get; set; } = new List<TenderDocument>();
    }
}
