using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Platform.Services
{
    public class BidService : IBidService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BidService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BidDto>> GetAllBidsAsync()
        {
            var bids = await _unitOfWork.Bids.GetAllAsync();
            return bids.Select(MapToDto);
        }

        public async Task<BidDto?> GetBidByIdAsync(Guid id)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(id);
            return bid != null ? MapToDto(bid) : null;
        }

        public async Task<IEnumerable<BidDto>> GetBidsBySupplierAsync(Guid supplierId)
        {
            var bids = await _unitOfWork.Bids.GetBidsBySupplierAsync(supplierId);
            return bids.Select(MapToDto);
        }

        public async Task<IEnumerable<BidDto>> GetBidsByTenderAsync(Guid tenderId)
        {
            var bids = await _unitOfWork.Bids.GetBidsByTenderAsync(tenderId);
            return bids.Select(MapToDto);
        }

        public async Task<BidDto> SubmitBidAsync(Guid supplierId, BidSubmissionDto dto)
        {
            // Validate tender exists and is open
            var tender = await _unitOfWork.Tenders.GetByIdAsync(dto.TenderId);
            if (tender == null)
                throw new ArgumentException("Tender not found");

            // Check if supplier already has a bid for this tender
            var existingBid = await _unitOfWork.Bids.GetBidBySupplierAndTenderAsync(supplierId, dto.TenderId);
            if (existingBid != null)
                throw new InvalidOperationException("Supplier already has a bid for this tender");

            var bid = new Bid
            {
                Amount = dto.Amount,
                Proposal = dto.Proposal,
                TenderId = dto.TenderId,
                SupplierId = supplierId,
                Status = BidStatus.Submitted,
                SubmittedAt = DateTime.UtcNow
            };

            await _unitOfWork.Bids.AddAsync(bid);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(bid);
        }

        public async Task<BidDto?> UpdateBidAsync(Guid id, UpdateBidDto dto)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(id);
            if (bid == null) return null;

            if (dto.Amount.HasValue) bid.Amount = dto.Amount.Value;
            if (!string.IsNullOrEmpty(dto.Proposal)) bid.Proposal = dto.Proposal;
            if (dto.Status.HasValue) bid.Status = dto.Status.Value;
            if (!string.IsNullOrEmpty(dto.Notes)) bid.Notes = dto.Notes;

            _unitOfWork.Bids.Update(bid);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(bid);
        }

        public async Task<bool> WithdrawBidAsync(Guid id, Guid supplierId)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(id);
            if (bid == null || bid.SupplierId != supplierId) return false;

            bid.Status = BidStatus.Withdrawn;
            _unitOfWork.Bids.Update(bid);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ReviewBidAsync(Guid id, BidStatus status, string? notes)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(id);
            if (bid == null) return false;

            bid.Status = status;
            bid.Notes = notes;
            bid.ReviewedAt = DateTime.UtcNow;

            _unitOfWork.Bids.Update(bid);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static BidDto MapToDto(Bid bid)
        {
            return new BidDto
            {
                Id = bid.Id,
                Amount = bid.Amount,
                Proposal = bid.Proposal,
                AttachmentPath = bid.AttachmentPath,
                Status = bid.Status,
                SubmittedAt = bid.SubmittedAt,
                Notes = bid.Notes,
                ReviewedAt = bid.ReviewedAt,
                SupplierId = bid.SupplierId,
                SupplierName = bid.Supplier?.User?.Name ?? "",
                TenderId = bid.TenderId,
                TenderTitle = bid.Tender?.Title ?? ""
            };
        }
    }
}
