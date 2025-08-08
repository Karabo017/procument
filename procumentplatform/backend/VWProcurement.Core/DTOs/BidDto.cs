using System.ComponentModel.DataAnnotations;
using VWProcurement.Core.Models;

namespace VWProcurement.Core.DTOs
{
    public class BidDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Proposal { get; set; }
        public string? AttachmentPath { get; set; }
        public BidStatus Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? Notes { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int TenderId { get; set; }
        public string TenderTitle { get; set; } = string.Empty;
    }

    public class CreateBidDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        public string? Proposal { get; set; }

        [Required]
        public int TenderId { get; set; }

        [Required]
        public int SupplierId { get; set; }
    }

    public class UpdateBidDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal? Amount { get; set; }

        public string? Proposal { get; set; }
        public BidStatus? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class BidSubmissionDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        public string? Proposal { get; set; }

        [Required]
        public int TenderId { get; set; }
    }
}
