namespace VWProcurement.Core.Models
{
    public class TenderAmendment
    {
        public Guid Id { get; set; }
        public Guid TenderId { get; set; }
        public int AmendmentNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AmendmentDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        public Tender Tender { get; set; } = null!;
    }
}