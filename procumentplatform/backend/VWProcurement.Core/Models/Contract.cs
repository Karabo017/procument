namespace VWProcurement.Core.Models
{
    public class Contract
    {
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public Guid TenderId { get; set; }
        public Guid WinningBidId { get; set; }
        public Guid SupplierId { get; set; }
        public Guid BuyerId { get; set; }
        public decimal ContractValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";
        public string? PaymentSchedule { get; set; }
        public string? DeliverySchedule { get; set; }
        public decimal? PerformanceGuarantee { get; set; }
        public string? ContractTerms { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public Tender Tender { get; set; } = null!;
        public Bid WinningBid { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;
        public Buyer Buyer { get; set; } = null!;
    }
}