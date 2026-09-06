using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.Core.Services;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto);
    Task<ApiResponse<UserDto>> RegisterAsync(CreateUserDto dto);
    Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
    Task<ApiResponse<List<UserDto>>> GetAllUsersAsync();
    string GenerateToken(UserDto user);
    bool VerifyPassword(string password, string hash);
    string HashPassword(string password);
}
