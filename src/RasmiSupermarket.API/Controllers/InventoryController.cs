using Microsoft.AspNetCore.Mvc;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Services;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    [HttpPost("movement")]
    public async Task<ActionResult<ApiResponse<InventoryDto>>> RecordMovement([FromBody] CreateInventoryMovementDto dto)
    {
        _logger.LogInformation($"Recording inventory movement for product {dto.ProductId}");
        var userId = User?.FindFirst("sub")?.Value ?? "unknown";
        var response = await _inventoryService.RecordMovementAsync(dto, userId);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("history/{productId}")]
    public async Task<ActionResult<ApiResponse<List<InventoryDto>>>> GetHistory(int productId)
    {
        _logger.LogInformation($"Getting inventory history for product {productId}");
        var response = await _inventoryService.GetInventoryHistoryAsync(productId);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpGet("report")]
    public async Task<ActionResult<ApiResponse<List<InventoryReportDto>>>> GetReport()
    {
        _logger.LogInformation("Getting inventory report");
        var response = await _inventoryService.GetInventoryReportAsync();
        return Ok(response);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<ApiResponse<List<InventoryReportDto>>>> GetLowStockItems()
    {
        _logger.LogInformation("Getting low stock items");
        var response = await _inventoryService.GetLowStockItemsAsync();
        return Ok(response);
    }

    [HttpGet("out-of-stock")]
    public async Task<ActionResult<ApiResponse<List<InventoryReportDto>>>> GetOutOfStockItems()
    {
        _logger.LogInformation("Getting out of stock items");
        var response = await _inventoryService.GetOutOfStockItemsAsync();
        return Ok(response);
    }

    [HttpGet("total-value")]
    public async Task<ActionResult<ApiResponse<decimal>>> GetTotalValue()
    {
        _logger.LogInformation("Getting total inventory value");
        var response = await _inventoryService.GetInventoryValueAsync();
        return Ok(response);
    }

    [HttpPost("movement-analysis")]
    public async Task<ActionResult<ApiResponse<List<StockMovementAnalysisDto>>>> GetMovementAnalysis(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        _logger.LogInformation($"Getting stock movement analysis from {startDate} to {endDate}");
        var response = await _inventoryService.GetStockMovementAnalysisAsync(startDate, endDate);
        return Ok(response);
    }

    [HttpPost("audit")]
    public async Task<ActionResult<ApiResponse<InventoryAuditDto>>> PerformAudit(
        [FromQuery] int productId,
        [FromQuery] int physicalCount,
        [FromQuery] string reason)
    {
        _logger.LogInformation($"Performing inventory audit for product {productId}");
        var response = await _inventoryService.PerformInventoryAuditAsync(productId, physicalCount, reason);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
