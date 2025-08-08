namespace VWProcurement.Core.Models
{
    public class TenderCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public TenderCategory? ParentCategory { get; set; }
        public List<TenderCategory> SubCategories { get; set; } = new();
        public List<Tender> Tenders { get; set; } = new();
    }
}
