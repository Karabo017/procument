using Microsoft.EntityFrameworkCore.Storage;
using VWProcurement.Core.Interfaces;
using VWProcurement.Data.Repositories;

namespace VWProcurement.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VWProcurementDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(VWProcurementDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Suppliers = new SupplierRepository(_context);
            Buyers = new BuyerRepository(_context);
            Managers = new ManagerRepository(_context);
            Tenders = new TenderRepository(_context);
            Bids = new BidRepository(_context);
        }

        public IUserRepository Users { get; }
        public ISupplierRepository Suppliers { get; }
        public IBuyerRepository Buyers { get; }
        public IManagerRepository Managers { get; }
        public ITenderRepository Tenders { get; }
        public IBidRepository Bids { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
