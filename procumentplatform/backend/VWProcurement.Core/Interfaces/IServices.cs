using VWProcurement.Core.DTOs;
using VWProcurement.Core.Models;

namespace VWProcurement.Core.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto?> GetSupplierByIdAsync(int id);
        Task<SupplierDto?> GetSupplierByEmailAsync(string email);
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
        Task<SupplierDto?> UpdateSupplierAsync(int id, UpdateSupplierDto dto);
        Task<bool> DeleteSupplierAsync(int id);
        Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync();
    }

    public interface IBuyerService
    {
        Task<IEnumerable<BuyerDto>> GetAllBuyersAsync();
        Task<BuyerDto?> GetBuyerByIdAsync(int id);
        Task<BuyerDto?> GetBuyerByEmailAsync(string email);
        Task<BuyerDto> CreateBuyerAsync(CreateBuyerDto dto);
        Task<BuyerDto?> UpdateBuyerAsync(int id, UpdateBuyerDto dto);
        Task<bool> DeleteBuyerAsync(int id);
        Task<IEnumerable<BuyerDto>> GetActiveBuyersAsync();
    }

    public interface ITenderService
    {
        Task<IEnumerable<TenderDto>> GetAllTendersAsync();
        Task<TenderDto?> GetTenderByIdAsync(int id);
        Task<IEnumerable<TenderDto>> GetOpenTendersAsync();
        Task<IEnumerable<TenderDto>> GetTendersByBuyerAsync(int buyerId);
        Task<TenderDto> CreateTenderAsync(CreateTenderDto dto);
        Task<TenderDto?> UpdateTenderAsync(int id, UpdateTenderDto dto);
        Task<bool> DeleteTenderAsync(int id);
        Task<bool> PublishTenderAsync(int id, PublishTenderDto dto);
        Task<bool> CloseTenderAsync(int id);
        Task<bool> AwardTenderAsync(int tenderId, int bidId);
    }

    public interface IBidService
    {
        Task<IEnumerable<BidDto>> GetAllBidsAsync();
        Task<BidDto?> GetBidByIdAsync(int id);
        Task<IEnumerable<BidDto>> GetBidsBySupplierAsync(int supplierId);
        Task<IEnumerable<BidDto>> GetBidsByTenderAsync(int tenderId);
        Task<BidDto> SubmitBidAsync(int supplierId, BidSubmissionDto dto);
        Task<BidDto?> UpdateBidAsync(int id, UpdateBidDto dto);
        Task<bool> WithdrawBidAsync(int id, int supplierId);
        Task<bool> ReviewBidAsync(int id, BidStatus status, string? notes);
    }
}
