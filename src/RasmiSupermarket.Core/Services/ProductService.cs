using AutoMapper;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Entities;
using RasmiSupermarket.Core.Exceptions;
using RasmiSupermarket.Core.Responses;
using RasmiSupermarket.Data.Repositories;

namespace RasmiSupermarket.Core.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} not found");
                return ApiResponse<ProductDto>.ErrorResponse($"Product with id {id} not found");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return ApiResponse<ProductDto>.SuccessResponse(productDto, "Product retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting product: {ex.Message}");
            return ApiResponse<ProductDto>.ErrorResponse($"Error getting product: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<ProductDto>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return ApiResponse<List<ProductDto>>.SuccessResponse(productDtos, "Products retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting all products: {ex.Message}");
            return ApiResponse<List<ProductDto>>.ErrorResponse($"Error getting products: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<ProductDto>>> GetProductsByCategoryAsync(string category)
    {
        try
        {
            var products = await _unitOfWork.Products.FindAsync(p => p.Category == category && p.IsActive);
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return ApiResponse<List<ProductDto>>.SuccessResponse(productDtos, "Products retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting products by category: {ex.Message}");
            return ApiResponse<List<ProductDto>>.ErrorResponse($"Error getting products: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto)
    {
        try
        {
            // Check if SKU already exists
            var existingProduct = await _unitOfWork.Products.SingleOrDefaultAsync(p => p.SKU == productDto.SKU);
            if (existingProduct != null)
            {
                return ApiResponse<ProductDto>.ErrorResponse($"Product with SKU {productDto.SKU} already exists");
            }

            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow;
            
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<ProductDto>(product);
            return ApiResponse<ProductDto>.SuccessResponse(resultDto, "Product created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating product: {ex.Message}");
            return ApiResponse<ProductDto>.ErrorResponse($"Error creating product: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, ProductDto productDto)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.ErrorResponse($"Product with id {id} not found");
            }

            // Check if SKU is being changed and if new SKU already exists
            if (product.SKU != productDto.SKU)
            {
                var existingProduct = await _unitOfWork.Products.SingleOrDefaultAsync(p => p.SKU == productDto.SKU);
                if (existingProduct != null)
                {
                    return ApiResponse<ProductDto>.ErrorResponse($"Product with SKU {productDto.SKU} already exists");
                }
            }

            _mapper.Map(productDto, product);
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<ProductDto>(product);
            return ApiResponse<ProductDto>.SuccessResponse(resultDto, "Product updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating product: {ex.Message}");
            return ApiResponse<ProductDto>.ErrorResponse($"Error updating product: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return ApiResponse<bool>.ErrorResponse($"Product with id {id} not found");
            }

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Product deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting product: {ex.Message}");
            return ApiResponse<bool>.ErrorResponse($"Error deleting product: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<ProductDto>>> GetLowStockProductsAsync()
    {
        try
        {
            var products = await _unitOfWork.Products.FindAsync(
                p => p.StockQuantity <= p.ReorderLevel && p.IsActive);
            
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return ApiResponse<List<ProductDto>>.SuccessResponse(productDtos, "Low stock products retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting low stock products: {ex.Message}");
            return ApiResponse<List<ProductDto>>.ErrorResponse($"Error getting low stock products: {ex.Message}");
        }
    }
}
