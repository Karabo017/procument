using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Data.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(VWProcurementDbContext context) : base(context)
        {
        }

        public async Task<Supplier?> GetByEmailAsync(string email)
        {
            return await _context.Suppliers
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.User.Email.ToLower() == email.ToLower());
        }

        public async Task<Supplier?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Suppliers
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliers()
        {
            return await _context.Suppliers
                .Include(s => s.User)
                .Where(s => s.User.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersWithBidsAsync()
        {
            return await _context.Suppliers
                .Include(s => s.User)
                .Include(s => s.Bids)
                .Where(s => s.Bids.Any())
                .ToListAsync();
        }
    }
}
