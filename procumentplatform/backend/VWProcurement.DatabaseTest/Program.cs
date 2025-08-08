using Microsoft.EntityFrameworkCore;
using VWProcurement.Data;
using VWProcurement.Core.Models;

// Create a simple test of our database
var connectionString = "Server=(localdb)\\mssqllocaldb;Database=VWProcurementDb;Trusted_Connection=true;MultipleActiveResultSets=true";

var options = new DbContextOptionsBuilder<VWProcurementDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var context = new VWProcurementDbContext(options);

Console.WriteLine("🔍 Testing VW Procurement Database Connection...");

try
{
    // Test database connection
    var canConnect = await context.Database.CanConnectAsync();
    Console.WriteLine($"✅ Database Connection: {(canConnect ? "SUCCESS" : "FAILED")}");

    if (canConnect)
    {
        // Show table counts
        var userCount = await context.Users.CountAsync();
        var supplierCount = await context.Suppliers.CountAsync();
        var buyerCount = await context.Buyers.CountAsync();
        var managerCount = await context.Managers.CountAsync();
        var tenderCount = await context.Tenders.CountAsync();
        var bidCount = await context.Bids.CountAsync();
        var categoryCount = await context.TenderCategories.CountAsync();

        Console.WriteLine("\n📊 Database Table Status:");
        Console.WriteLine($"   Users: {userCount}");
        Console.WriteLine($"   Suppliers: {supplierCount}");
        Console.WriteLine($"   Buyers: {buyerCount}");
        Console.WriteLine($"   Managers: {managerCount}");
        Console.WriteLine($"   Tenders: {tenderCount}");
        Console.WriteLine($"   Bids: {bidCount}");
        Console.WriteLine($"   Tender Categories: {categoryCount}");

        // Create a test tender category
        if (categoryCount == 0)
        {
            Console.WriteLine("\n➕ Creating test data...");
            
            var category = new TenderCategory
            {
                Id = Guid.NewGuid(),
                Name = "IT Services",
                Description = "Information Technology Services and Solutions",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.TenderCategories.Add(category);
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Test tender category created!");
        }

        // Create a test user
        if (userCount == 0)
        {
            Console.WriteLine("➕ Creating test user...");
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@vwprocurement.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                UserType = "Manager",
                IsActive = true,
                EmailVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Create corresponding manager record
            var manager = new Manager
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = "Test",
                LastName = "Manager",
                Department = "IT",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Managers.Add(manager);
            await context.SaveChangesAsync();
            
            Console.WriteLine("✅ Test user and manager created!");
        }

        Console.WriteLine("\n🎉 Database migration and setup completed successfully!");
        Console.WriteLine("🚀 VW Procurement Platform is ready for development!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    Console.WriteLine($"📍 Stack Trace: {ex.StackTrace}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();
