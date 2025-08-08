namespace VWProcurement.Core.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid GeneratedBy { get; set; }
        public string? Parameters { get; set; }
        public string? FilePath { get; set; }
        public string Status { get; set; } = "Generated";
        public DateTime CreatedAt { get; set; }
        
        public User GeneratedByUser { get; set; } = null!;
    }
}