using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Data.Repositories
{
    public class BuyerRepository : Repository<Buyer>, IBuyerRepository
    {
        public BuyerRepository(VWProcurementDbContext context) : base(context)
        {
        }

        public async Task<Buyer?> GetByEmailAsync(string email)
        {
            return await _context.Buyers
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.User.Email.ToLower() == email.ToLower());
        }

        public async Task<Buyer?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Buyers
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task<IEnumerable<Buyer>> GetActiveBuyers()
        {
            return await _context.Buyers
                .Include(b => b.User)
                .Where(b => b.User.Status == UserStatus.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Buyer>> GetBuyersWithTendersAsync()
        {
            return await _context.Buyers
                .Include(b => b.User)
                .Include(b => b.Tenders)
                .Where(b => b.Tenders.Any())
                .ToListAsync();
        }
    }
}
