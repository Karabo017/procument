namespace VWProcurement.Core.Models
{
    public class TenderQA
    {
        public Guid Id { get; set; }
        public Guid TenderId { get; set; }
        public Guid? SupplierId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string? Answer { get; set; }
        public bool IsPublic { get; set; } = true;
        public DateTime QuestionDate { get; set; }
        public Guid? AnsweredBy { get; set; }
        public DateTime? AnswerDate { get; set; }
        
        public Tender Tender { get; set; } = null!;
        public Supplier? Supplier { get; set; }
        public User? AnsweredByUser { get; set; }
    }
}