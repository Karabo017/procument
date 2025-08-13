using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.Models
{
    public enum DocumentType
    {
        TenderDocument = 0,    // Documents attached to tender
        BidDocument = 1,       // Documents submitted with bid
        EvaluationDocument = 2, // Documents used in evaluation
        AwardDocument = 3      // Documents related to award
    }
    
    public class TenderDocument
    {
        public Guid Id { get; set; }
        
        public Guid TenderId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? ContentType { get; set; }
        
        public long FileSize { get; set; }
        
        public DocumentType Type { get; set; } = DocumentType.TenderDocument;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsPublic { get; set; } = true; // Visible to all suppliers
        
        public Guid UploadedBy { get; set; }
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Tender Tender { get; set; } = null!;
        public virtual User UploadedByUser { get; set; } = null!;
    }
}
