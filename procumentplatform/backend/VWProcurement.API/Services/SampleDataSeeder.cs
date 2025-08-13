using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Models;
using VWProcurement.Data;

namespace VWProcurement.API.Services
{
    public class SampleDataSeeder
    {
        public class SeedSummary
        {
            public int Categories { get; set; }
            public int Users { get; set; }
            public int Buyers { get; set; }
            public int Managers { get; set; }
            public int TendersCreated { get; set; }
        }

        public async Task<SeedSummary> SeedAsync(VWProcurementDbContext _context)
        {
            // Upsert sample categories
            var categoryNames = new[] { "IT & Technology", "Supplies & Equipment", "Professional Services", "Construction", "Maintenance" };
            var existingCategories = await _context.TenderCategories.ToListAsync();
            var categoriesDict = existingCategories.ToDictionary(c => c.Name, c => c);
            foreach (var name in categoryNames)
            {
                if (!categoriesDict.ContainsKey(name))
                {
                    var c = new TenderCategory { Id = Guid.NewGuid(), Name = name, IsActive = true, CreatedAt = DateTime.UtcNow };
                    _context.TenderCategories.Add(c);
                    categoriesDict[name] = c;
                }
            }
            await _context.SaveChangesAsync();

            // Upsert manager user
            var managerEmail = "admin@procurementplatform.com";
            var managerUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == managerEmail);
            if (managerUser == null)
            {
                managerUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = managerEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.PlatformManager,
                    Status = UserStatus.Active,
                    FirstName = "Platform",
                    LastName = "Administrator",
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Users.Add(managerUser);
                await _context.SaveChangesAsync();
            }

            // Upsert manager profile
            var existingManager = await _context.Managers.FirstOrDefaultAsync(m => m.UserId == managerUser.Id);
            if (existingManager == null)
            {
                var manager = new Manager
                {
                    Id = Guid.NewGuid(),
                    UserId = managerUser.Id,
                    FirstName = "John",
                    LastName = "Administrator",
                    User = managerUser
                };
                _context.Managers.Add(manager);
                await _context.SaveChangesAsync();
            }

            // Upsert buyers and buyer users
            var buyerSeed = new[]
            {
                new { Email = "procurement@company.co.za", FirstName = "Company", LastName = "Procurement", Org = "Corporate South Africa" },
                new { Email = "supply@corporategroup.com", FirstName = "Corporate Group", LastName = "Supply", Org = "Corporate Group Procurement" }
            };
            var buyerUserByEmail = new Dictionary<string, User>();
            foreach (var b in buyerSeed)
            {
                var u = await _context.Users.FirstOrDefaultAsync(x => x.Email == b.Email);
                if (u == null)
                {
                    u = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = b.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("buyer123"),
                        Role = UserRole.Buyer,
                        Status = UserStatus.Active,
                        FirstName = b.FirstName,
                        LastName = b.LastName,
                        EmailVerified = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Users.Add(u);
                    await _context.SaveChangesAsync();
                }
                buyerUserByEmail[b.Email] = u;

                var buyerProfile = await _context.Buyers.FirstOrDefaultAsync(x => x.UserId == u.Id);
                if (buyerProfile == null)
                {
                    buyerProfile = new Buyer
                    {
                        Id = Guid.NewGuid(),
                        UserId = u.Id,
                        OrganizationName = b.Org,
                        IsVerified = true,
                        User = u
                    };
                    _context.Buyers.Add(buyerProfile);
                    await _context.SaveChangesAsync();
                }
            }

            // Create sample tenders if missing
            var tenders = new List<Tender>
            {
                new Tender
                {
                    Id = Guid.NewGuid(),
                    TenderNumber = "TND-2025-001",
                    Title = "Enterprise Software Licensing and Support",
                    Description = "Procurement of comprehensive enterprise software licensing including Microsoft 365, Adobe Creative Cloud, and associated support services for all organization operations.",
                    Requirements = "Must include: 1. Microsoft Office 365 E5 licenses for 2,500 users; 2. Adobe Creative Suite licenses for 150 users; 3. 24/7 technical support; 4. Migration services from current systems; 5. Training for 50 IT staff members; 6. Compliance with organizational security standards.",
                    EstimatedValue = 4500000.00m,
                    Status = TenderStatus.Open,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5),
                    PublishDate = DateTime.UtcNow.AddDays(-8),
                    ClosingDate = DateTime.UtcNow.AddDays(15),
                    BuyerId = buyerUserByEmail["procurement@company.co.za"].Id,
                    CategoryId = categoriesDict["IT & Technology"].Id,
                    Category = categoriesDict["IT & Technology"]
                },
                new Tender
                {
                    Id = Guid.NewGuid(),
                    TenderNumber = "TND-2025-002",
                    Title = "Manufacturing Equipment Maintenance Services",
                    Description = "Comprehensive preventive and corrective maintenance services for manufacturing equipment across production facilities in South Africa.",
                    Requirements = "Service must cover: 1. All stamping press equipment; 2. Body shop welding robots; 3. Paint booth systems; 4. Final assembly line equipment; 5. 24/7 emergency breakdown support; 6. Predictive maintenance using IoT sensors; 7. Monthly performance reports; 8. Spare parts management.",
                    EstimatedValue = 12000000.00m,
                    Status = TenderStatus.Open,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3),
                    PublishDate = DateTime.UtcNow.AddDays(-12),
                    ClosingDate = DateTime.UtcNow.AddDays(20),
                    BuyerId = buyerUserByEmail["procurement@company.co.za"].Id,
                    CategoryId = categoriesDict["Maintenance"].Id,
                    Category = categoriesDict["Maintenance"]
                },
                new Tender
                {
                    Id = Guid.NewGuid(),
                    TenderNumber = "TND-2025-003",
                    Title = "Corporate Catering and Facilities Management",
                    Description = "Comprehensive catering services and facilities management for offices and manufacturing sites across South Africa.",
                    Requirements = "Services include: 1. Daily catering for 3,000+ employees; 2. Executive dining services; 3. Event catering capabilities; 4. Facilities cleaning and maintenance; 5. Security services; 6. Landscaping and grounds maintenance; 7. Waste management and recycling; 8. HACCP and food safety compliance.",
                    EstimatedValue = 8500000.00m,
                    Status = TenderStatus.Open,
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2),
                    PublishDate = DateTime.UtcNow.AddDays(-10),
                    ClosingDate = DateTime.UtcNow.AddDays(25),
                    BuyerId = buyerUserByEmail["supply@corporategroup.com"].Id,
                    CategoryId = categoriesDict["Professional Services"].Id,
                    Category = categoriesDict["Professional Services"]
                },
                new Tender
                {
                    Id = Guid.NewGuid(),
                    TenderNumber = "TND-2025-004",
                    Title = "Office Furniture and Equipment Supply",
                    Description = "Supply of modern office furniture, equipment, and ergonomic solutions for a new administrative building and facility upgrades.",
                    Requirements = "Supply requirements: 1. 500 ergonomic office chairs; 2. 200 height-adjustable desks; 3. Meeting room furniture for 25 rooms; 4. Reception area furniture; 5. Breakout space furniture; 6. Storage solutions; 7. Installation and setup services; 8. 5-year warranty on all items; 9. Sustainable and eco-friendly materials preferred.",
                    EstimatedValue = 3200000.00m,
                    Status = TenderStatus.Open,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1),
                    PublishDate = DateTime.UtcNow.AddDays(-18),
                    ClosingDate = DateTime.UtcNow.AddDays(3),
                    BuyerId = buyerUserByEmail["procurement@company.co.za"].Id,
                    CategoryId = categoriesDict["Supplies & Equipment"].Id,
                    Category = categoriesDict["Supplies & Equipment"]
                },
                new Tender
                {
                    Id = Guid.NewGuid(),
                    TenderNumber = "TND-2025-005",
                    Title = "Digital Marketing and Brand Management Services",
                    Description = "Comprehensive digital marketing services including social media management, content creation, SEO, and brand management.",
                    Requirements = "Service scope: 1. Social media management across all platforms; 2. Content creation (video, graphics, articles); 3. SEO and SEM management; 4. Brand guideline development; 5. Website maintenance and optimization; 6. Influencer partnership management; 7. Market research and analytics; 8. Crisis communication planning; 9. Quarterly performance reviews.",
                    EstimatedValue = 2800000.00m,
                    Status = TenderStatus.Open,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1),
                    PublishDate = DateTime.UtcNow.AddDays(-6),
                    ClosingDate = DateTime.UtcNow.AddDays(18),
                    BuyerId = buyerUserByEmail["supply@corporategroup.com"].Id,
                    CategoryId = categoriesDict["Professional Services"].Id,
                    Category = categoriesDict["Professional Services"]
                }
            };

            int createdTenders = 0;
            foreach (var t in tenders)
            {
                var exists = await _context.Tenders.AnyAsync(x => x.TenderNumber == t.TenderNumber);
                if (!exists)
                {
                    _context.Tenders.Add(t);
                    createdTenders++;
                }
            }
            await _context.SaveChangesAsync();

            return new SeedSummary
            {
                Categories = categoriesDict.Count,
                Users = await _context.Users.CountAsync(),
                Buyers = await _context.Buyers.CountAsync(),
                Managers = await _context.Managers.CountAsync(),
                TendersCreated = createdTenders
            };
        }
    }
}
