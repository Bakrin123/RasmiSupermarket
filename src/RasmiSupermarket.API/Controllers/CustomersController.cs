using Microsoft.AspNetCore.Mvc;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Services;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CustomerDto>>>> GetAll()
    {
        _logger.LogInformation("Getting all customers");
        var response = await _customerService.GetAllCustomersAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetById(int id)
    {
        _logger.LogInformation($"Getting customer with id: {id}");
        var response = await _customerService.GetCustomerByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Create([FromBody] CreateCustomerDto dto)
    {
        _logger.LogInformation("Creating new customer");
        var response = await _customerService.CreateCustomerAsync(dto);
        return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response) : BadRequest(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        _logger.LogInformation($"Updating customer with id: {id}");
        var response = await _customerService.UpdateCustomerAsync(id, dto);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        _logger.LogInformation($"Deleting customer with id: {id}");
        var response = await _customerService.DeleteCustomerAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost("{customerId}/loyalty-points")]
    public async Task<ActionResult<ApiResponse<bool>>> AddLoyaltyPoints(int customerId, [FromBody] decimal points)
    {
        _logger.LogInformation($"Adding loyalty points to customer: {customerId}");
        var response = await _customerService.AddLoyaltyPointsAsync(customerId, points);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
