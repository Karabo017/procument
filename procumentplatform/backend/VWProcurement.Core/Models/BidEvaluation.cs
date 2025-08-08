namespace VWProcurement.Core.Models
{
    public class BidEvaluation
    {
        public Guid Id { get; set; }
        public Guid BidId { get; set; }
        public Guid CriteriaId { get; set; }
        public decimal Score { get; set; }
        public string? Comments { get; set; }
        public Guid EvaluatedBy { get; set; }
        public DateTime EvaluatedAt { get; set; }
        
        public Bid Bid { get; set; } = null!;
        public BidEvaluationCriteria Criteria { get; set; } = null!;
        public User EvaluatedByUser { get; set; } = null!;
    }
}