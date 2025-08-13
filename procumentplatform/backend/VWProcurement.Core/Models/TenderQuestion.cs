using System.ComponentModel.DataAnnotations;

namespace VWProcurement.Core.Models
{
    public class TenderQuestion
    {
        public Guid Id { get; set; }
        
        public Guid TenderId { get; set; }
        
        public Guid UserId { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Question { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Answer { get; set; }
        
        public Guid? AnsweredBy { get; set; }
        
        public DateTime QuestionDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? AnswerDate { get; set; }
        
        public bool IsPublic { get; set; } = true; // Visible to all suppliers
        
        // Navigation properties
        public virtual Tender Tender { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual User? AnsweredByUser { get; set; }
    }
}
