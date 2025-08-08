using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VWProcurement.Data
{
    public class VWProcurementDbContextFactory : IDesignTimeDbContextFactory<VWProcurementDbContext>
    {
        public VWProcurementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VWProcurementDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VWProcurementDb;Trusted_Connection=true;MultipleActiveResultSets=true");
            
            return new VWProcurementDbContext(optionsBuilder.Options);
        }
    }
}
