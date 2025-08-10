using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Data.Repositories
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        public BidRepository(VWProcurementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Bid>> GetBidsByTenderAsync(Guid tenderId)
        {
            return await _context.Bids
                .Include(b => b.Supplier)
                .ThenInclude(s => s.User)
                .Include(b => b.Tender)
                .Where(b => b.TenderId == tenderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetBidsBySupplierAsync(Guid supplierId)
        {
            return await _context.Bids
                .Include(b => b.Supplier)
                .ThenInclude(s => s.User)
                .Include(b => b.Tender)
                .Where(b => b.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<Bid?> GetBidBySupplierAndTenderAsync(Guid supplierId, Guid tenderId)
        {
            return await _context.Bids
                .Include(b => b.Supplier)
                .ThenInclude(s => s.User)
                .Include(b => b.Tender)
                .FirstOrDefaultAsync(b => b.SupplierId == supplierId && b.TenderId == tenderId);
        }

        public async Task<IEnumerable<Bid>> GetBidsByStatusAsync(BidStatus status)
        {
            return await _context.Bids
                .Include(b => b.Supplier)
                .ThenInclude(s => s.User)
                .Include(b => b.Tender)
                .Where(b => b.Status == status)
                .ToListAsync();
        }
    }
}
