using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.Core.Services;

public interface IProductService
{
    Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
    Task<ApiResponse<List<ProductDto>>> GetAllProductsAsync();
    Task<ApiResponse<List<ProductDto>>> GetProductsByCategoryAsync(string category);
    Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto);
    Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, ProductDto productDto);
    Task<ApiResponse<bool>> DeleteProductAsync(int id);
    Task<ApiResponse<List<ProductDto>>> GetLowStockProductsAsync();
}
