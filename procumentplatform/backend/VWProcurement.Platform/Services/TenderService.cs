using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Platform.Services
{
    public class TenderService : ITenderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TenderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TenderDto>> GetAllTendersAsync()
        {
            var tenders = await _unitOfWork.Tenders.GetTendersWithBidsAsync();
            return tenders.Select(MapToDto);
        }

        public async Task<TenderDto?> GetTenderByIdAsync(Guid id)
        {
            var tender = await _unitOfWork.Tenders.GetTenderWithBidsAsync(id);
            return tender != null ? MapToDto(tender) : null;
        }

        public async Task<IEnumerable<TenderDto>> GetOpenTendersAsync()
        {
            var tenders = await _unitOfWork.Tenders.GetOpenTendersAsync();
            return tenders.Select(MapToDto);
        }

        public async Task<IEnumerable<TenderDto>> GetTendersByBuyerAsync(Guid buyerId)
        {
            var tenders = await _unitOfWork.Tenders.GetTendersByBuyerAsync(buyerId);
            return tenders.Select(MapToDto);
        }

        public async Task<TenderDto> CreateTenderAsync(CreateTenderDto dto)
        {
            var tender = new Tender
            {
                Title = dto.Title,
                Description = dto.Description,
                Requirements = dto.Requirements,
                EstimatedValue = dto.EstimatedValue,
                ClosingDate = dto.ClosingDate,
                BuyerId = dto.BuyerId,
                Status = TenderStatus.Draft.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Tenders.AddAsync(tender);
            await _unitOfWork.SaveChangesAsync();

            // Reload to get buyer info
            var createdTender = await _unitOfWork.Tenders.GetTenderWithBidsAsync(tender.Id);
            return MapToDto(createdTender!);
        }

        public async Task<TenderDto?> UpdateTenderAsync(Guid id, UpdateTenderDto dto)
        {
            var tender = await _unitOfWork.Tenders.GetByIdAsync(id);
            if (tender == null) return null;

            if (!string.IsNullOrEmpty(dto.Title)) tender.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description)) tender.Description = dto.Description;
            if (dto.Requirements != null) tender.Requirements = dto.Requirements;
            if (dto.EstimatedValue.HasValue) tender.EstimatedValue = dto.EstimatedValue;
            if (dto.Status.HasValue) tender.Status = dto.Status.Value.ToString();
            if (dto.ClosingDate.HasValue) tender.ClosingDate = dto.ClosingDate.Value;
            if (dto.ApprovedByManagerId.HasValue) tender.ApprovedByManagerId = dto.ApprovedByManagerId;
            
            tender.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Tenders.Update(tender);
            await _unitOfWork.SaveChangesAsync();

            var updatedTender = await _unitOfWork.Tenders.GetTenderWithBidsAsync(id);
            return MapToDto(updatedTender!);
        }

        public async Task<bool> DeleteTenderAsync(int id)
        {
            var tender = await _unitOfWork.Tenders.GetByIdAsync(id);
            if (tender == null) return false;

            // Check if tender has bids
            var bids = await _unitOfWork.Bids.GetBidsByTenderAsync(id);
            if (bids.Any())
            {
                return false; // Cannot delete tender with bids
            }

            _unitOfWork.Tenders.Remove(tender);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PublishTenderAsync(int id, PublishTenderDto dto)
        {
            var tender = await _unitOfWork.Tenders.GetByIdAsync(id);
            if (tender == null || tender.Status != TenderStatus.Draft) return false;

            tender.Status = TenderStatus.Open;
            tender.PublishedAt = DateTime.UtcNow;
            if (dto.ClosingDate.HasValue) tender.ClosingDate = dto.ClosingDate;
            tender.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Tenders.Update(tender);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseTenderAsync(int id)
        {
            var tender = await _unitOfWork.Tenders.GetByIdAsync(id);
            if (tender == null || tender.Status != TenderStatus.Open) return false;

            tender.Status = TenderStatus.Closed;
            tender.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Tenders.Update(tender);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AwardTenderAsync(int tenderId, int bidId)
        {
            var tender = await _unitOfWork.Tenders.GetByIdAsync(tenderId);
            var bid = await _unitOfWork.Bids.GetByIdAsync(bidId);

            if (tender == null || bid == null || bid.TenderId != tenderId || 
                tender.Status != TenderStatus.Closed) return false;

            tender.Status = TenderStatus.Awarded;
            tender.AwardDate = DateTime.UtcNow;
            tender.UpdatedAt = DateTime.UtcNow;

            bid.Status = BidStatus.Awarded;
            bid.UpdatedAt = DateTime.UtcNow;

            // Reject other bids
            var otherBids = await _unitOfWork.Bids.GetBidsByTenderAsync(tenderId);
            foreach (var otherBid in otherBids.Where(b => b.Id != bidId))
            {
                otherBid.Status = BidStatus.Rejected;
                otherBid.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Bids.Update(otherBid);
            }

            _unitOfWork.Tenders.Update(tender);
            _unitOfWork.Bids.Update(bid);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static TenderDto MapToDto(Tender tender)
        {
            return new TenderDto
            {
                Id = tender.Id,
                Title = tender.Title,
                Description = tender.Description,
                Requirements = tender.Requirements,
                EstimatedValue = tender.EstimatedValue,
                Status = tender.Status,
                CreatedAt = tender.CreatedAt,
                PublishedAt = tender.PublishedAt,
                ClosingDate = tender.ClosingDate,
                AwardDate = tender.AwardDate,
                BuyerId = tender.BuyerId,
                BuyerName = tender.Buyer?.Name ?? "",
                ApprovedByManagerId = tender.ApprovedByManagerId,
                ApprovedByManagerName = tender.ApprovedByManager?.Name,
                BidsCount = tender.Bids?.Count ?? 0
            };
        }
    }
}
