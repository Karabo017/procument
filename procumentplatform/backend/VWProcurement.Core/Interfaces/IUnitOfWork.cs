namespace VWProcurement.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISupplierRepository Suppliers { get; }
        IBuyerRepository Buyers { get; }
        IManagerRepository Managers { get; }
        ITenderRepository Tenders { get; }
        IBidRepository Bids { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
