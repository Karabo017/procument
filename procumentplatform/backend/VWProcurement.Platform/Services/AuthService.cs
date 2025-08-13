using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;
using BCrypt.Net;

namespace VWProcurement.Platform.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _jwtSecret = _configuration["JwtSettings:SecretKey"] ?? "your-super-secret-jwt-key-that-should-be-at-least-32-characters-long";
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email
                var users = await _unitOfWork.Users.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Email.ToLower() == loginDto.Email.ToLower());

                if (user == null || user.Status != UserStatus.Active)
                {
                    return new AuthResultDto
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password"
                    };
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return new AuthResultDto
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password"
                    };
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Generate JWT token
                var userDto = await MapToUserDtoAsync(user);
                var token = GenerateJwtToken(userDto);

                return new AuthResultDto
                {
                    IsSuccess = true,
                    Token = token,
                    User = userDto,
                    Message = "Login successful",
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto, string role)
        {
            try
            {
                // Check if passwords match
                if (registerDto.Password != registerDto.ConfirmPassword)
                {
                    return new AuthResultDto
                    {
                        IsSuccess = false,
                        Message = "Passwords do not match"
                    };
                }

                // Check if user already exists
                var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email.ToLower());
                if (existingUser != null)
                {
                    return new AuthResultDto
                    {
                        IsSuccess = false,
                        Message = "User with this email already exists"
                    };
                }

                // Create new user
                var user = new User
                {
                    Email = registerDto.Email.ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Role = Enum.Parse<UserRole>(role),
                    FirstName = registerDto.Name?.Split(' ').FirstOrDefault() ?? "",
                    LastName = registerDto.Name?.Split(' ').Skip(1).FirstOrDefault() ?? "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = UserStatus.Active,
                    EmailVerified = true
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Create role-specific entity
                switch (role.ToLower())
                {
                    case "supplier":
                        var supplier = new Supplier
                        {
                            UserId = user.Id,
                            CompanyName = registerDto.CompanyName ?? "",
                            BusinessRegistrationNumber = "", // Will be updated later
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.Suppliers.AddAsync(supplier);
                        break;
                    
                    case "buyer":
                        var buyer = new Buyer
                        {
                            UserId = user.Id,
                            OrganizationName = registerDto.CompanyName ?? "",
                            OrganizationType = "Government", // Default, can be updated
                            ContactPerson = registerDto.Name ?? "",
                            BusinessAddress = "", // Will be updated later
                            City = "",
                            Province = "",
                            PostalCode = "",
                            Country = "South Africa",
                            AuthorizedSignatory = registerDto.Name ?? "",
                            ProcurementAuthority = "Standard",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.Buyers.AddAsync(buyer);
                        break;
                    
                    case "manager":
                        var manager = new Manager
                        {
                            UserId = user.Id,
                            FirstName = registerDto.Name?.Split(' ').FirstOrDefault() ?? "",
                            LastName = registerDto.Name?.Split(' ').LastOrDefault() ?? "",
                            Department = "General",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.Managers.AddAsync(manager);
                        break;
                }

                await _unitOfWork.SaveChangesAsync();

                var userDto = await MapToUserDtoAsync(user);
                var token = GenerateJwtToken(userDto);

                return new AuthResultDto
                {
                    IsSuccess = true,
                    Token = token,
                    User = userDto,
                    Message = "Registration successful",
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return false;

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                    return false;

                // Check if new passwords match
                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
                    return false;

                // Update password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return user == null ? null : await MapToUserDtoAsync(user);
        }

        public async Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto updateDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return null;

                // Check if email is being changed and if it's already taken
                if (user.Email.ToLower() != updateDto.Email.ToLower())
                {
                    var users = await _unitOfWork.Users.GetAllAsync();
                    var existingUser = users.FirstOrDefault(u => u.Email.ToLower() == updateDto.Email.ToLower());
                    if (existingUser != null)
                        return null; // Email already taken
                }

                // Update user properties
                if (!string.IsNullOrEmpty(updateDto.Name))
                {
                    var nameParts = updateDto.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    user.FirstName = nameParts.FirstOrDefault() ?? "";
                    user.LastName = nameParts.Skip(1).FirstOrDefault() ?? "";
                }
                user.Email = updateDto.Email.ToLower();
                user.CompanyName = updateDto.CompanyName;
                user.PhoneNumber = updateDto.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;

                // Handle profile photo if provided
                if (!string.IsNullOrEmpty(updateDto.ProfilePhotoBase64))
                {
                    // Save profile photo and update URL
                    user.ProfilePhotoPath = await SaveProfilePhotoAsync(userId, updateDto.ProfilePhotoBase64);
                }

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return await MapToUserDtoAsync(user);
            }
            catch
            {
                return null;
            }
        }

        public string GenerateJwtToken(UserDto user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name ?? ""),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserType ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(_jwtSecret);
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync(Guid userId)
        {
            // For now, just update last logout time if needed
            // In a production app, you might want to blacklist the token
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task<UserDto> MapToUserDtoAsync(User user)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserType = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                IsActive = user.Status == UserStatus.Active,
                Name = !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName) 
                    ? $"{user.FirstName} {user.LastName}"
                    : user.CompanyName ?? user.Email
            };

            // Get additional information based on user role
            switch (user.Role)
            {
                case UserRole.Supplier:
                    var supplier = await _unitOfWork.Suppliers.GetByUserIdAsync(user.Id);
                    if (supplier != null)
                    {
                        userDto.CompanyName = supplier.CompanyName;
                        userDto.Name = supplier.CompanyName; // Use company name as display name
                    }
                    break;
                
                case UserRole.Buyer:
                    var buyer = await _unitOfWork.Buyers.GetByUserIdAsync(user.Id);
                    if (buyer != null)
                    {
                        userDto.CompanyName = buyer.OrganizationName;
                        userDto.Name = buyer.ContactPerson;
                    }
                    break;
                
                case UserRole.PlatformManager:
                    var manager = await _unitOfWork.Managers.GetByUserIdAsync(user.Id);
                    if (manager != null)
                    {
                        userDto.Name = $"{manager.FirstName} {manager.LastName}";
                        userDto.CompanyName = "VW Procurement"; // Default for managers
                    }
                    break;
                    
                case UserRole.PublicViewer:
                default:
                    userDto.CompanyName = "Public User";
                    break;
            }

            return userDto;
        }

        private async Task<string> SaveProfilePhotoAsync(Guid userId, string base64Image)
        {
            try
            {
                // Create uploads directory if it doesn't exist
                var uploadsPath = Path.Combine("wwwroot", "uploads", "profiles");
                Directory.CreateDirectory(uploadsPath);

                // Generate unique filename
                var fileName = $"profile_{userId}_{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Convert base64 to bytes and save
                var imageBytes = Convert.FromBase64String(base64Image.Split(',').Last());
                await File.WriteAllBytesAsync(filePath, imageBytes);

                return $"/uploads/profiles/{fileName}";
            }
            catch
            {
                return "";
            }
        }
    }
}
