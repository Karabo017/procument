using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VWProcurement.Data;
using VWProcurement.Core.Models;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly VWProcurementDbContext _context;

        public HealthController(VWProcurementDbContext context)
        {
            _context = context;
        }

        [HttpGet("database")]
        public async Task<ActionResult> CheckDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    return Ok(new { 
                        status = "error", 
                        message = "Cannot connect to database",
                        database = "disconnected"
                    });
                }

                var stats = new
                {
                    status = "healthy",
                    message = "Database connection successful",
                    database = "connected",
                    tables = new
                    {
                        users = await _context.Users.CountAsync(),
                        suppliers = await _context.Suppliers.CountAsync(),
                        buyers = await _context.Buyers.CountAsync(),
                        managers = await _context.Managers.CountAsync(),
                        tenders = await _context.Tenders.CountAsync(),
                        bids = await _context.Bids.CountAsync(),
                        categories = await _context.TenderCategories.CountAsync()
                    }
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    status = "error", 
                    message = ex.Message,
                    database = "error"
                });
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new {
                        u.Id,
                        u.Email,
                        u.UserType,
                        u.IsActive,
                        u.EmailVerified,
                        u.CreatedAt
                    })
                    .Take(10)
                    .ToListAsync();

                return Ok(new { 
                    status = "success",
                    count = users.Count,
                    data = users
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    status = "error", 
                    message = ex.Message
                });
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                var categories = await _context.TenderCategories
                    .Where(c => c.IsActive)
                    .Select(c => new {
                        c.Id,
                        c.Name,
                        c.Description,
                        c.IsActive,
                        c.CreatedAt
                    })
                    .ToListAsync();

                return Ok(new { 
                    status = "success",
                    count = categories.Count,
                    data = categories
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    status = "error", 
                    message = ex.Message
                });
            }
        }

        [HttpPost("test-user")]
        public async Task<ActionResult> CreateTestUser()
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == "test@frontend.com");

                if (existingUser != null)
                {
                    return Ok(new { 
                        status = "exists",
                        message = "Test user already exists",
                        userId = existingUser.Id
                    });
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "test@frontend.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    UserType = "Manager",
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    status = "created",
                    message = "Test user created successfully",
                    userId = user.Id,
                    email = user.Email
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    status = "error", 
                    message = ex.Message
                });
            }
        }
    }
}
