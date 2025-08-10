using VWProcurement.Core.DTOs;
using VWProcurement.Core.Models;

namespace VWProcurement.Core.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto?> GetSupplierByIdAsync(Guid id);
        Task<SupplierDto?> GetSupplierByEmailAsync(string email);
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
        Task<SupplierDto?> UpdateSupplierAsync(Guid id, UpdateSupplierDto dto);
        Task<bool> DeleteSupplierAsync(Guid id);
        Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync();
    }

    public interface IBuyerService
    {
        Task<IEnumerable<BuyerDto>> GetAllBuyersAsync();
        Task<BuyerDto?> GetBuyerByIdAsync(Guid id);
        Task<BuyerDto?> GetBuyerByEmailAsync(string email);
        Task<BuyerDto> CreateBuyerAsync(CreateBuyerDto dto);
        Task<BuyerDto?> UpdateBuyerAsync(Guid id, UpdateBuyerDto dto);
        Task<bool> DeleteBuyerAsync(Guid id);
        Task<IEnumerable<BuyerDto>> GetActiveBuyersAsync();
    }

    public interface ITenderService
    {
        Task<IEnumerable<TenderDto>> GetAllTendersAsync();
        Task<TenderDto?> GetTenderByIdAsync(Guid id);
        Task<IEnumerable<TenderDto>> GetOpenTendersAsync();
        Task<IEnumerable<TenderDto>> GetTendersByBuyerAsync(Guid buyerId);
        Task<TenderDto> CreateTenderAsync(CreateTenderDto dto);
        Task<TenderDto?> UpdateTenderAsync(Guid id, UpdateTenderDto dto);
        Task<bool> DeleteTenderAsync(Guid id);
        Task<bool> PublishTenderAsync(Guid id, PublishTenderDto dto);
        Task<bool> CloseTenderAsync(Guid id);
        Task<bool> AwardTenderAsync(Guid tenderId, Guid bidId);
    }

    public interface IBidService
    {
        Task<IEnumerable<BidDto>> GetAllBidsAsync();
        Task<BidDto?> GetBidByIdAsync(Guid id);
        Task<IEnumerable<BidDto>> GetBidsBySupplierAsync(Guid supplierId);
        Task<IEnumerable<BidDto>> GetBidsByTenderAsync(Guid tenderId);
        Task<BidDto> SubmitBidAsync(Guid supplierId, BidSubmissionDto dto);
        Task<BidDto?> UpdateBidAsync(Guid id, UpdateBidDto dto);
        Task<bool> WithdrawBidAsync(Guid id, Guid supplierId);
        Task<bool> ReviewBidAsync(Guid id, BidStatus status, string? notes);
    }
}
