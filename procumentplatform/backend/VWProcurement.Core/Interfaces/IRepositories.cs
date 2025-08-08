using VWProcurement.Core.Models;

namespace VWProcurement.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(string role);
        Task<IEnumerable<User>> GetActiveUsersAsync();
    }

    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier?> GetByEmailAsync(string email);
        Task<Supplier?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Supplier>> GetActiveSuppliers();
        Task<IEnumerable<Supplier>> GetSuppliersWithBidsAsync();
    }

    public interface IBuyerRepository : IRepository<Buyer>
    {
        Task<Buyer?> GetByEmailAsync(string email);
        Task<Buyer?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Buyer>> GetActiveBuyers();
        Task<IEnumerable<Buyer>> GetBuyersWithTendersAsync();
    }

    public interface IManagerRepository : IRepository<Manager>
    {
        Task<Manager?> GetByEmailAsync(string email);
        Task<Manager?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Manager>> GetActiveManagers();
        Task<IEnumerable<Manager>> GetManagersByDepartmentAsync(string department);
    }

    public interface ITenderRepository : IRepository<Tender>
    {
        Task<IEnumerable<Tender>> GetOpenTendersAsync();
        Task<IEnumerable<Tender>> GetTendersByBuyerAsync(Guid buyerId);
        Task<IEnumerable<Tender>> GetTendersWithBidsAsync();
        Task<Tender?> GetTenderWithBidsAsync(Guid tenderId);
        Task<IEnumerable<Tender>> GetTendersByStatusAsync(TenderStatus status);
    }

    public interface IBidRepository : IRepository<Bid>
    {
        Task<IEnumerable<Bid>> GetBidsBySupplierAsync(Guid supplierId);
        Task<IEnumerable<Bid>> GetBidsByTenderAsync(Guid tenderId);
        Task<Bid?> GetBidBySupplierAndTenderAsync(Guid supplierId, Guid tenderId);
        Task<IEnumerable<Bid>> GetBidsByStatusAsync(BidStatus status);
    }
}
