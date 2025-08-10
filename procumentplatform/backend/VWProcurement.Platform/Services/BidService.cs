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
            // Stub implementation
            throw new NotImplementedException("SubmitBidAsync not yet implemented");
        }

        public async Task<BidDto?> UpdateBidAsync(Guid id, UpdateBidDto dto)
        {
            // Stub implementation
            throw new NotImplementedException("UpdateBidAsync not yet implemented");
        }

        public async Task<bool> WithdrawBidAsync(Guid id, Guid supplierId)
        {
            // Stub implementation
            throw new NotImplementedException("WithdrawBidAsync not yet implemented");
        }

        public async Task<bool> ReviewBidAsync(Guid id, BidStatus status, string? notes)
        {
            // Stub implementation
            throw new NotImplementedException("ReviewBidAsync not yet implemented");
        }
            return bids.Select(MapToDto);
        }

        public async Task<IEnumerable<BidDto>> GetBidsByTenderAsync(int tenderId)
        {
            var bids = await _unitOfWork.Bids.GetBidsByTenderAsync(tenderId);
            return bids.Select(MapToDto);
        }

        public async Task<BidDto> SubmitBidAsync(int supplierId, BidSubmissionDto dto)
        {
            // Check if tender exists and is open
            var tender = await _unitOfWork.Tenders.GetByIdAsync(dto.TenderId);
            if (tender == null || tender.Status != TenderStatus.Open)
                throw new InvalidOperationException("Tender is not available for bidding");

            // Check if closing date has passed
            if (tender.ClosingDate.HasValue && tender.ClosingDate < DateTime.UtcNow)
                throw new InvalidOperationException("Tender has already closed");

            // Check if supplier already has a bid for this tender
            var existingBid = await _unitOfWork.Bids.GetBidBySupplierAndTenderAsync(supplierId, dto.TenderId);
            if (existingBid != null)
                throw new InvalidOperationException("Supplier has already submitted a bid for this tender");

            var bid = new Bid
            {
                Amount = dto.Amount,
                Proposal = dto.Proposal,
                TenderId = dto.TenderId,
                SupplierId = supplierId,
                Status = BidStatus.Submitted,
                SubmittedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Bids.AddAsync(bid);
            await _unitOfWork.SaveChangesAsync();

            // Reload to get related data
            var submittedBid = await _unitOfWork.Bids.GetBidBySupplierAndTenderAsync(supplierId, dto.TenderId);
            return MapToDto(submittedBid!);
        }

        public async Task<BidDto?> UpdateBidAsync(int id, UpdateBidDto dto)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(id);
            if (bid == null) return null;

            // Only allow updates if bid is still in submitted status
            if (bid.Status != BidStatus.Submitted)
                throw new InvalidOperationException("Cannot update bid that is already under review or processed");

            if (dto.Amount.HasValue) bid.Amount = dto.Amount.Value;
            if (dto.Proposal != null) bid.Proposal = dto.Proposal;
            if (dto.Status.HasValue) bid.Status = dto.Status.Value;
            if (dto.Notes != null) bid.Notes = dto.Notes;
            
            bid.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Bids.Update(bid);
            await _unitOfWork.SaveChangesAsync();

            var updatedBid = await _unitOfWork.Bids.GetBidBySupplierAndTenderAsync(bid.SupplierId, bid.TenderId);
            return MapToDto(updatedBid!);
        }

        public async Task<bool> WithdrawBidAsync(int id, int supplierId)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(id);
            if (bid == null || bid.SupplierId != supplierId) return false;

            // Only allow withdrawal if bid is still in submitted status
            if (bid.Status != BidStatus.Submitted) return false;

            _unitOfWork.Bids.Remove(bid);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReviewBidAsync(int id, BidStatus status, string? notes)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(id);
            if (bid == null) return false;

            bid.Status = status;
            bid.Notes = notes;
            bid.ReviewedAt = DateTime.UtcNow;
            bid.UpdatedAt = DateTime.UtcNow;

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
                SupplierName = bid.Supplier?.Name ?? "",
                TenderId = bid.TenderId,
                TenderTitle = bid.Tender?.Title ?? ""
            };
        }
    }
}
