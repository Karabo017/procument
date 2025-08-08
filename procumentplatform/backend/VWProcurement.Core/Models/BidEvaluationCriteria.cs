namespace VWProcurement.Core.Models
{
    public class BidEvaluationCriteria
    {
        public Guid Id { get; set; }
        public Guid TenderId { get; set; }
        public string CriteriaName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; } = 100;
        public string CriteriaType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        public Tender Tender { get; set; } = null!;
    }
}