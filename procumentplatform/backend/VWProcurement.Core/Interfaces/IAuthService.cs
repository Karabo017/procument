using VWProcurement.Core.DTOs;

namespace VWProcurement.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto, string role);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto updateDto);
        string GenerateJwtToken(UserDto user);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync(Guid userId);
    }
}
