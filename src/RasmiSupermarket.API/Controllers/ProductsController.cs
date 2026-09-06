using Microsoft.AspNetCore.Mvc;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Services;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetAll()
    {
        _logger.LogInformation("Getting all products");
        var response = await _productService.GetAllProductsAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
    {
        _logger.LogInformation($"Getting product with id: {id}");
        var response = await _productService.GetProductByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetByCategory(string category)
    {
        _logger.LogInformation($"Getting products by category: {category}");
        var response = await _productService.GetProductsByCategoryAsync(category);
        return Ok(response);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetLowStockProducts()
    {
        _logger.LogInformation("Getting low stock products");
        var response = await _productService.GetLowStockProductsAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromBody] ProductDto dto)
    {
        _logger.LogInformation("Creating new product");
        var response = await _productService.CreateProductAsync(dto);
        return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response) : BadRequest(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, [FromBody] ProductDto dto)
    {
        _logger.LogInformation($"Updating product with id: {id}");
        var response = await _productService.UpdateProductAsync(id, dto);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        _logger.LogInformation($"Deleting product with id: {id}");
        var response = await _productService.DeleteProductAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }
}
