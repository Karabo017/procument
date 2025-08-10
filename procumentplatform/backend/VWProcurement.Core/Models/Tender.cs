using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VWProcurement.Core.Models
{
    public enum TenderStatus
    {
        Draft,
        Open,
        Closed,
        Awarded,
        Cancelled
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
        
        [StringLength(50)]
        public string Status { get; set; } = "Draft";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? PublishDate { get; set; }
        
        public DateTime ClosingDate { get; set; }
        
        public DateTime? AwardDate { get; set; }
        
        public DateTime? PublishedAt { get; set; }
        
        public Guid? ApprovedByManagerId { get; set; }
        
        // Foreign Keys
        public Guid BuyerId { get; set; }
        public Guid CategoryId { get; set; }
        
        // Navigation Properties
    public virtual Buyer Buyer { get; set; } = null!;
    public virtual TenderCategory Category { get; set; } = null!;
    public virtual Manager? ApprovedByManager { get; set; }
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
