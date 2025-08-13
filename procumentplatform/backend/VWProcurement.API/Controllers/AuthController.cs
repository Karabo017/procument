using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VWProcurement.Core.Models;
using VWProcurement.Data;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly VWProcurementDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AuthController(VWProcurementDbContext context, IConfiguration configuration, IWebHostEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Email and password are required" });
                }

                // Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

                if (user == null)
                {
                    // Development fallback account to unblock login when DB is empty/unavailable
                    if (_env.IsDevelopment() && request.Email.Equals("admin@procurementplatform.com", StringComparison.OrdinalIgnoreCase)
                        && request.Password == "admin123")
                    {
                        var devUser = new User
                        {
                            Id = Guid.NewGuid(),
                            Email = request.Email,
                            Role = UserRole.PlatformManager,
                            Status = UserStatus.Active,
                            EmailVerified = true
                        };
                        var tokenDev = GenerateJwtToken(devUser);
                        return Ok(new
                        {
                            success = true,
                            message = "Login successful",
                            token = tokenDev,
                            user = new
                            {
                                id = devUser.Id,
                                email = devUser.Email,
                                userType = devUser.Role.ToString(),
                                isActive = true,
                                emailVerified = true,
                                profile = new { firstName = "Platform", lastName = "Administrator" }
                            }
                        });
                    }

                    return Unauthorized(new { message = "Invalid email or password" });
                }

                // Check if user is active
                if (user.Status != UserStatus.Active)
                {
                    return Unauthorized(new { message = "Account is inactive" });
                }

                // Verify password (using BCrypt)
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                // Get user profile data based on user type
                object? profile = null;
                switch (user.Role)
                {
                    case UserRole.Supplier:
                        profile = await _context.Suppliers
                            .FirstOrDefaultAsync(s => s.UserId == user.Id);
                        break;
                    case UserRole.Buyer:
                        profile = await _context.Buyers
                            .FirstOrDefaultAsync(b => b.UserId == user.Id);
                        break;
                    case UserRole.PlatformManager:
                        profile = await _context.Managers
                            .FirstOrDefaultAsync(m => m.UserId == user.Id);
                        break;
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    success = true,
                    message = "Login successful",
                    token = token,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        userType = user.Role.ToString(),
                        isActive = user.Status == UserStatus.Active,
                        emailVerified = user.EmailVerified,
                        profile = profile
                    }
                });
            }
            catch (Exception ex)
            {
                // Last-resort Development fallback when DB errors
                if (_env.IsDevelopment() && request.Email.Equals("admin@procurementplatform.com", StringComparison.OrdinalIgnoreCase)
                    && request.Password == "admin123")
                {
                    var devUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = request.Email,
                        Role = UserRole.PlatformManager,
                        Status = UserStatus.Active,
                        EmailVerified = true
                    };
                    var tokenDev = GenerateJwtToken(devUser);
                    return Ok(new
                    {
                        success = true,
                        message = "Login successful",
                        token = tokenDev,
                        user = new
                        {
                            id = devUser.Id,
                            email = devUser.Email,
                            userType = devUser.Role.ToString(),
                            isActive = true,
                            emailVerified = true,
                            profile = new { firstName = "Platform", lastName = "Administrator" }
                        }
                    });
                }

                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }

        [HttpPost("register/manager")]
        public async Task<ActionResult> RegisterManager([FromBody] ManagerRegistrationRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

                if (existingUser != null)
                {
                    return BadRequest(new { message = "User with this email already exists" });
                }

                // Create user
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = UserRole.PlatformManager,
                    Status = UserStatus.Active,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                // Create manager profile
                var manager = new Manager
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    User = user
                };

                _context.Managers.Add(manager);

                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    success = true,
                    message = "Manager registered successfully",
                    token = token,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        userType = user.Role.ToString(),
                        profile = new
                        {
                            id = manager.Id,
                            firstName = manager.FirstName,
                            lastName = manager.LastName
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JwtSettings:SecretKey"] ?? "vw-procurement-super-secret-jwt-key-for-token-generation-2024"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim("userType", user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ManagerRegistrationRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
