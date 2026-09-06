using Microsoft.AspNetCore.Mvc;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Services;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto dto)
    {
        _logger.LogInformation($"User login attempt: {dto.Username}");
        var response = await _authService.LoginAsync(dto);
        return response.Success ? Ok(response) : Unauthorized(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] CreateUserDto dto)
    {
        _logger.LogInformation($"User registration attempt: {dto.Username}");
        var response = await _authService.RegisterAsync(dto);
        return response.Success ? CreatedAtAction("GetUser", new { id = response.Data?.Id }, response) : BadRequest(response);
    }

    [HttpGet("users/{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(int id)
    {
        _logger.LogInformation($"Getting user with id: {id}");
        var response = await _authService.GetUserByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpGet("users")]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAllUsers()
    {
        _logger.LogInformation("Getting all users");
        var response = await _authService.GetAllUsersAsync();
        return Ok(response);
    }
}
