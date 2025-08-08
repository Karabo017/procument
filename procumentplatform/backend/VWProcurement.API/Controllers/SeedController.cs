using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Models;
using VWProcurement.Data;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly VWProcurementDbContext _context;

        public SeedController(VWProcurementDbContext context)
        {
            _context = context;
        }

        [HttpPost("sample-data")]
        public async Task<ActionResult> SeedSampleData()
        {
            try
            {
                // Check if data already exists
                var existingTenders = await _context.Tenders.AnyAsync();
                if (existingTenders)
                {
                    return Ok(new { message = "Sample data already exists" });
                }

                // Create sample categories
                var categories = new List<TenderCategory>
                {
                    new TenderCategory { Id = Guid.NewGuid(), Name = "IT & Technology", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new TenderCategory { Id = Guid.NewGuid(), Name = "Supplies & Equipment", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new TenderCategory { Id = Guid.NewGuid(), Name = "Professional Services", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new TenderCategory { Id = Guid.NewGuid(), Name = "Construction", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new TenderCategory { Id = Guid.NewGuid(), Name = "Maintenance", IsActive = true, CreatedAt = DateTime.UtcNow }
                };

                _context.TenderCategories.AddRange(categories);
                await _context.SaveChangesAsync();

                // Create sample manager
                var managerUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@vwprocurement.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    UserType = "Manager",
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(managerUser);

                var manager = new Manager
                {
                    Id = Guid.NewGuid(),
                    UserId = managerUser.Id,
                    FirstName = "John",
                    LastName = "Administrator",
                    User = managerUser
                };

                _context.Managers.Add(manager);

                // Create sample buyers
                var buyerUsers = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "procurement@volkswagen.co.za",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("buyer123"),
                        UserType = "Buyer",
                        IsActive = true,
                        EmailVerified = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "supply@vwgroup.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("buyer123"),
                        UserType = "Buyer",
                        IsActive = true,
                        EmailVerified = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                _context.Users.AddRange(buyerUsers);

                var buyers = new List<Buyer>
                {
                    new Buyer
                    {
                        Id = Guid.NewGuid(),
                        UserId = buyerUsers[0].Id,
                        OrganizationName = "Volkswagen South Africa",
                        IsVerified = true,
                        User = buyerUsers[0]
                    },
                    new Buyer
                    {
                        Id = Guid.NewGuid(),
                        UserId = buyerUsers[1].Id,
                        OrganizationName = "VW Group Procurement",
                        IsVerified = true,
                        User = buyerUsers[1]
                    }
                };

                _context.Buyers.AddRange(buyers);
                await _context.SaveChangesAsync();

                // Create sample tenders with comprehensive information
                var tenders = new List<Tender>
                {
                    new Tender
                    {
                        Id = Guid.NewGuid(),
                        TenderNumber = "VW-2025-001",
                        Title = "Enterprise Software Licensing and Support",
                        Description = "Procurement of comprehensive enterprise software licensing including Microsoft Office 365, Adobe Creative Suite, and associated support services for all VW SA operations.",
                        Requirements = "Must include: 1. Microsoft Office 365 E5 licenses for 2,500 users; 2. Adobe Creative Suite licenses for 150 users; 3. 24/7 technical support; 4. Migration services from current systems; 5. Training for 50 IT staff members; 6. Compliance with VW security standards.",
                        EstimatedValue = 4500000.00m,
                        Status = "Open",
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        UpdatedAt = DateTime.UtcNow.AddDays(-5),
                        PublishDate = DateTime.UtcNow.AddDays(-8),
                        ClosingDate = DateTime.UtcNow.AddDays(15),
                        BuyerId = buyers[0].Id,
                        CategoryId = categories[0].Id, // IT & Technology
                        Buyer = buyers[0],
                        Category = categories[0]
                    },
                    new Tender
                    {
                        Id = Guid.NewGuid(),
                        TenderNumber = "VW-2025-002",
                        Title = "Manufacturing Equipment Maintenance Services",
                        Description = "Comprehensive preventive and corrective maintenance services for manufacturing equipment across all VW production facilities in South Africa.",
                        Requirements = "Service must cover: 1. All stamping press equipment; 2. Body shop welding robots; 3. Paint booth systems; 4. Final assembly line equipment; 5. 24/7 emergency breakdown support; 6. Predictive maintenance using IoT sensors; 7. Monthly performance reports; 8. Spare parts management.",
                        EstimatedValue = 12000000.00m,
                        Status = "Open",
                        CreatedAt = DateTime.UtcNow.AddDays(-15),
                        UpdatedAt = DateTime.UtcNow.AddDays(-3),
                        PublishDate = DateTime.UtcNow.AddDays(-12),
                        ClosingDate = DateTime.UtcNow.AddDays(20),
                        BuyerId = buyers[0].Id,
                        CategoryId = categories[4].Id, // Maintenance
                        Buyer = buyers[0],
                        Category = categories[4]
                    },
                    new Tender
                    {
                        Id = Guid.NewGuid(),
                        TenderNumber = "VW-2025-003",
                        Title = "Corporate Catering and Facilities Management",
                        Description = "Comprehensive catering services and facilities management for VW Group offices and manufacturing sites across South Africa.",
                        Requirements = "Services include: 1. Daily catering for 3,000+ employees; 2. Executive dining services; 3. Event catering capabilities; 4. Facilities cleaning and maintenance; 5. Security services; 6. Landscaping and grounds maintenance; 7. Waste management and recycling; 8. HACCP and food safety compliance.",
                        EstimatedValue = 8500000.00m,
                        Status = "Open",
                        CreatedAt = DateTime.UtcNow.AddDays(-12),
                        UpdatedAt = DateTime.UtcNow.AddDays(-2),
                        PublishDate = DateTime.UtcNow.AddDays(-10),
                        ClosingDate = DateTime.UtcNow.AddDays(25),
                        BuyerId = buyers[1].Id,
                        CategoryId = categories[2].Id, // Professional Services
                        Buyer = buyers[1],
                        Category = categories[2]
                    },
                    new Tender
                    {
                        Id = Guid.NewGuid(),
                        TenderNumber = "VW-2025-004",
                        Title = "Office Furniture and Equipment Supply",
                        Description = "Supply of modern office furniture, equipment, and ergonomic solutions for new VW administrative building and facility upgrades.",
                        Requirements = "Supply requirements: 1. 500 ergonomic office chairs; 2. 200 height-adjustable desks; 3. Meeting room furniture for 25 rooms; 4. Reception area furniture; 5. Breakout space furniture; 6. Storage solutions; 7. Installation and setup services; 8. 5-year warranty on all items; 9. Sustainable and eco-friendly materials preferred.",
                        EstimatedValue = 3200000.00m,
                        Status = "Closing Soon",
                        CreatedAt = DateTime.UtcNow.AddDays(-20),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        PublishDate = DateTime.UtcNow.AddDays(-18),
                        ClosingDate = DateTime.UtcNow.AddDays(3),
                        BuyerId = buyers[0].Id,
                        CategoryId = categories[1].Id, // Supplies & Equipment
                        Buyer = buyers[0],
                        Category = categories[1]
                    },
                    new Tender
                    {
                        Id = Guid.NewGuid(),
                        TenderNumber = "VW-2025-005",
                        Title = "Digital Marketing and Brand Management Services",
                        Description = "Comprehensive digital marketing services including social media management, content creation, SEO, and brand management for VW SA.",
                        Requirements = "Service scope: 1. Social media management across all platforms; 2. Content creation (video, graphics, articles); 3. SEO and SEM management; 4. Brand guideline development; 5. Website maintenance and optimization; 6. Influencer partnership management; 7. Market research and analytics; 8. Crisis communication planning; 9. Quarterly performance reviews.",
                        EstimatedValue = 2800000.00m,
                        Status = "Open",
                        CreatedAt = DateTime.UtcNow.AddDays(-8),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        PublishDate = DateTime.UtcNow.AddDays(-6),
                        ClosingDate = DateTime.UtcNow.AddDays(18),
                        BuyerId = buyers[1].Id,
                        CategoryId = categories[2].Id, // Professional Services
                        Buyer = buyers[1],
                        Category = categories[2]
                    }
                };

                _context.Tenders.AddRange(tenders);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Sample data created successfully",
                    created = new
                    {
                        categories = categories.Count,
                        users = buyerUsers.Count + 1, // +1 for manager
                        buyers = buyers.Count,
                        managers = 1,
                        tenders = tenders.Count
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while creating sample data",
                    error = ex.Message
                });
            }
        }
    }
}
