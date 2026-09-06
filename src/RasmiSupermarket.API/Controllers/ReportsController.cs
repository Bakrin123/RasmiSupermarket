using Microsoft.AspNetCore.Mvc;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Services;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    [HttpGet("sales")]
    public async Task<ActionResult<ApiResponse<SalesReportDto>>> GetSalesReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        _logger.LogInformation($"Getting sales report from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
        var response = await _reportService.GetSalesReportAsync(startDate, endDate);
        return Ok(response);
    }

    [HttpGet("customers")]
    public async Task<ActionResult<ApiResponse<List<CustomerReportDto>>>> GetCustomerReport()
    {
        _logger.LogInformation("Getting customer report");
        var response = await _reportService.GetCustomerReportAsync();
        return Ok(response);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<List<CategoryReportDto>>>> GetCategoryReport()
    {
        _logger.LogInformation("Getting category report");
        var response = await _reportService.GetCategoryReportAsync();
        return Ok(response);
    }

    [HttpGet("dashboard-summary")]
    public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetDashboardSummary()
    {
        _logger.LogInformation("Getting dashboard summary");
        var response = await _reportService.GetDashboardSummaryAsync();
        return Ok(response);
    }

    [HttpGet("top-products")]
    public async Task<ActionResult<ApiResponse<List<ProductSalesDto>>>> GetTopProducts([FromQuery] int limit = 10)
    {
        _logger.LogInformation($"Getting top {limit} products");
        var response = await _reportService.GetTopProductsAsync(limit);
        return Ok(response);
    }

    [HttpGet("top-customers")]
    public async Task<ActionResult<ApiResponse<List<CustomerReportDto>>>> GetTopCustomers([FromQuery] int limit = 10)
    {
        _logger.LogInformation($"Getting top {limit} customers");
        var response = await _reportService.GetTopCustomersAsync(limit);
        return Ok(response);
    }

    [HttpGet("sales-export")]
    public async Task<ActionResult<ApiResponse<object>>> ExportSalesReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        _logger.LogInformation($"Exporting sales report from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
        var response = await _reportService.ExportSalesReportAsync(startDate, endDate);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
