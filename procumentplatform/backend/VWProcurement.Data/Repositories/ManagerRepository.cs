using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Data.Repositories
{
    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public ManagerRepository(VWProcurementDbContext context) : base(context)
        {
        }

        public async Task<Manager?> GetByEmailAsync(string email)
        {
            return await _context.Managers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.User.Email.ToLower() == email.ToLower());
        }

        public async Task<Manager?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Managers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<IEnumerable<Manager>> GetActiveManagers()
        {
            return await _context.Managers
                .Include(m => m.User)
                .Where(m => m.User.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Manager>> GetManagersByDepartmentAsync(string department)
        {
            return await _context.Managers
                .Include(m => m.User)
                .Where(m => m.Department == department)
                .ToListAsync();
        }
    }
}
