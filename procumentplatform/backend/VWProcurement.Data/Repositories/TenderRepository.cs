using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Data.Repositories
{
    public class TenderRepository : Repository<Tender>, ITenderRepository
    {
        public TenderRepository(VWProcurementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tender>> GetOpenTendersAsync()
        {
            return await _context.Tenders
                .Include(t => t.Buyer)
                .Include(t => t.Category)
                .Where(t => t.Status == TenderStatus.Open && t.ClosingDate > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetTendersByBuyerAsync(Guid buyerId)
        {
            return await _context.Tenders
                .Include(t => t.Buyer)
                .Include(t => t.Category)
                .Where(t => t.BuyerId == buyerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetTendersByStatusAsync(TenderStatus status)
        {
            return await _context.Tenders
                .Include(t => t.Buyer)
                .Include(t => t.Category)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<Tender?> GetTenderWithBidsAsync(Guid id)
        {
            return await _context.Tenders
                .Include(t => t.Buyer)
                .Include(t => t.Category)
                .Include(t => t.Bids)
                .ThenInclude(b => b.Supplier)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tender>> GetTendersWithBidsAsync()
        {
            return await _context.Tenders
                .Include(t => t.Buyer)
                .Include(t => t.Category)
                .Include(t => t.Bids)
                .Where(t => t.Bids.Any())
                .ToListAsync();
        }
    }
}
