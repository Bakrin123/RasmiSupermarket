using Microsoft.AspNetCore.Mvc;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Services;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ILogger<SuppliersController> _logger;

    public SuppliersController(ILogger<SuppliersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SupplierDto>>>> GetAll()
    {
        _logger.LogInformation("Getting all suppliers");
        // Implementation placeholder
        return Ok(ApiResponse<List<SupplierDto>>.SuccessResponse(new List<SupplierDto>(), "Suppliers retrieved"));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> GetById(int id)
    {
        _logger.LogInformation($"Getting supplier with id: {id}");
        // Implementation placeholder
        return Ok(ApiResponse<SupplierDto>.SuccessResponse(null, "Supplier retrieved"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> Create([FromBody] CreateSupplierDto dto)
    {
        _logger.LogInformation("Creating new supplier");
        // Implementation placeholder
        return CreatedAtAction(nameof(GetById), new { id = 1 }, 
            ApiResponse<SupplierDto>.SuccessResponse(null, "Supplier created"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> Update(int id, [FromBody] UpdateSupplierDto dto)
    {
        _logger.LogInformation($"Updating supplier with id: {id}");
        // Implementation placeholder
        return Ok(ApiResponse<SupplierDto>.SuccessResponse(null, "Supplier updated"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        _logger.LogInformation($"Deleting supplier with id: {id}");
        // Implementation placeholder
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Supplier deleted"));
    }
}
