using System.ComponentModel.DataAnnotations;
using VWProcurement.Core.Models;

namespace VWProcurement.Core.DTOs
{
    public class TenderDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Requirements { get; set; }
        public decimal? EstimatedValue { get; set; }
        public TenderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? AwardDate { get; set; }
        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public Guid? ApprovedByManagerId { get; set; }
        public string? ApprovedByManagerName { get; set; }
        public int BidsCount { get; set; }
    }

    public class CreateTenderDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? Requirements { get; set; }
        public decimal? EstimatedValue { get; set; }
        public DateTime? ClosingDate { get; set; }
        public Guid BuyerId { get; set; }
    }

    public class UpdateTenderDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public decimal? EstimatedValue { get; set; }
        public TenderStatus? Status { get; set; }
        public DateTime? ClosingDate { get; set; }
        public Guid? ApprovedByManagerId { get; set; }
    }

    public class PublishTenderDto
    {
        public DateTime? ClosingDate { get; set; }
    }
}
