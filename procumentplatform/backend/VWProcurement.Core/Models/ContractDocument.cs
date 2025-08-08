namespace VWProcurement.Core.Models
{
    public class ContractDocument
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        
        public Contract Contract { get; set; } = null!;
    }
}