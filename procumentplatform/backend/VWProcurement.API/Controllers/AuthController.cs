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

        public AuthController(VWProcurementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                // Check if user is active
                if (!user.IsActive)
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
                switch (user.UserType.ToLower())
                {
                    case "supplier":
                        profile = await _context.Suppliers
                            .FirstOrDefaultAsync(s => s.UserId == user.Id);
                        break;
                    case "buyer":
                        profile = await _context.Buyers
                            .FirstOrDefaultAsync(b => b.UserId == user.Id);
                        break;
                    case "manager":
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
                        userType = user.UserType,
                        isActive = user.IsActive,
                        emailVerified = user.EmailVerified,
                        profile = profile
                    }
                });
            }
            catch (Exception ex)
            {
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
                    UserType = "Manager",
                    IsActive = true,
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
                        userType = user.UserType,
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
                new Claim(ClaimTypes.Role, user.UserType),
                new Claim("userId", user.Id.ToString()),
                new Claim("userType", user.UserType)
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
