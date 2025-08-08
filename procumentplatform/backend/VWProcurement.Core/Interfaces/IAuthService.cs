using VWProcurement.Core.DTOs;

namespace VWProcurement.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto, string role);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserProfileDto updateDto);
        string GenerateJwtToken(UserDto user);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync(int userId);
    }
}
