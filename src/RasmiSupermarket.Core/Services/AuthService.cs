using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Entities;
using RasmiSupermarket.Core.Responses;
using RasmiSupermarket.Data.Repositories;

namespace RasmiSupermarket.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthService> logger, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);
            if (user == null)
            {
                _logger.LogWarning($"Login attempt failed for user: {loginDto.Username}");
                return ApiResponse<LoginResponseDto>.ErrorResponse("Invalid username or password");
            }

            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Invalid password for user: {loginDto.Username}");
                return ApiResponse<LoginResponseDto>.ErrorResponse("Invalid username or password");
            }

            user.LastLogin = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            var token = GenerateToken(userDto);

            var response = new LoginResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                Role = user.Role
            };

            _logger.LogInformation($"User logged in successfully: {loginDto.Username}");
            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during login: {ex.Message}");
            return ApiResponse<LoginResponseDto>.ErrorResponse($"Error during login: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> RegisterAsync(CreateUserDto dto)
    {
        try
        {
            // Check if username already exists
            var existingUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
            if (existingUser != null)
            {
                return ApiResponse<UserDto>.ErrorResponse($"Username '{dto.Username}' is already taken");
            }

            // Check if email already exists
            var existingEmail = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (existingEmail != null)
            {
                return ApiResponse<UserDto>.ErrorResponse($"Email '{dto.Email}' is already registered");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = HashPassword(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            _logger.LogInformation($"User registered successfully: {dto.Username}");
            return ApiResponse<UserDto>.SuccessResponse(userDto, "User registered successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during registration: {ex.Message}");
            return ApiResponse<UserDto>.ErrorResponse($"Error during registration: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<UserDto>.ErrorResponse($"User with id {id} not found");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.SuccessResponse(userDto, "User retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting user: {ex.Message}");
            return ApiResponse<UserDto>.ErrorResponse($"Error getting user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.IsActive);
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return ApiResponse<List<UserDto>>.SuccessResponse(userDtos, "Users retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting all users: {ex.Message}");
            return ApiResponse<List<UserDto>>.ErrorResponse($"Error getting users: {ex.Message}");
        }
    }

    public string GenerateToken(UserDto user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["AppSettings:JwtSecretKey"] ?? "default-secret-key-change-in-production"));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new System.Security.Claims.Claim("sub", user.Id.ToString()),
            new System.Security.Claims.Claim("username", user.Username),
            new System.Security.Claims.Claim("email", user.Email),
            new System.Security.Claims.Claim("role", user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: "RasmiSupermarket",
            audience: "RasmiSupermarketUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["AppSettings:JwtExpireMinutes"] ?? "1440")),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
