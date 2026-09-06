using AutoMapper;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Entities;
using RasmiSupermarket.Core.Responses;
using RasmiSupermarket.Data.Repositories;

namespace RasmiSupermarket.Core.Services;

public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InventoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<InventoryDto>> RecordMovementAsync(CreateInventoryMovementDto dto, string userId)
    {
        try
        {
            // Verify product exists
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                return ApiResponse<InventoryDto>.ErrorResponse($"Product with id {dto.ProductId} not found");
            }

            // Validate movement type
            var validMovementTypes = new[] { "In", "Out", "Adjustment" };
            if (!validMovementTypes.Contains(dto.MovementType))
            {
                return ApiResponse<InventoryDto>.ErrorResponse(
                    $"Invalid movement type. Allowed: {string.Join(", ", validMovementTypes)}");
            }

            // Create inventory movement record
            var inventory = new Inventory
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                MovementType = dto.MovementType,
                Reference = dto.Reference,
                Notes = dto.Notes,
                MovementDate = DateTime.UtcNow,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            // Update product stock
            switch (dto.MovementType)
            {
                case "In":
                    product.StockQuantity += dto.Quantity;
                    break;
                case "Out":
                    if (product.StockQuantity < dto.Quantity)
                    {
                        return ApiResponse<InventoryDto>.ErrorResponse(
                            $"Insufficient stock. Available: {product.StockQuantity}, Requested: {dto.Quantity}");
                    }
                    product.StockQuantity -= dto.Quantity;
                    break;
                case "Adjustment":
                    product.StockQuantity = dto.Quantity; // Direct adjustment
                    break;
            }

            product.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Inventories.AddAsync(inventory);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                $"Inventory movement recorded: Product {dto.ProductId}, Type: {dto.MovementType}, Qty: {dto.Quantity}");

            var inventoryDto = _mapper.Map<InventoryDto>(inventory);
            inventoryDto.ProductName = product.Name;

            return ApiResponse<InventoryDto>.SuccessResponse(inventoryDto, "Inventory movement recorded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error recording inventory movement: {ex.Message}");
            return ApiResponse<InventoryDto>.ErrorResponse($"Error recording movement: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<InventoryDto>>> GetInventoryHistoryAsync(int productId)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
            {
                return ApiResponse<List<InventoryDto>>.ErrorResponse($"Product with id {productId} not found");
            }

            var history = await _unitOfWork.Inventories.FindAsync(
                i => i.ProductId == productId);

            var historyDtos = _mapper.Map<List<InventoryDto>>(history);
            foreach (var item in historyDtos)
            {
                item.ProductName = product.Name;
            }

            return ApiResponse<List<InventoryDto>>.SuccessResponse(historyDtos, "Inventory history retrieved");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting inventory history: {ex.Message}");
            return ApiResponse<List<InventoryDto>>.ErrorResponse($"Error retrieving history: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<InventoryReportDto>>> GetInventoryReportAsync()
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var report = new List<InventoryReportDto>();

            foreach (var product in products)
            {
                var lastMovement = (await _unitOfWork.Inventories.FindAsync(
                    i => i.ProductId == product.Id)).OrderByDescending(i => i.MovementDate).FirstOrDefault();

                var status = GetStockStatus(product.StockQuantity, product.ReorderLevel);

                var reportItem = new InventoryReportDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    SKU = product.SKU,
                    CurrentStock = product.StockQuantity,
                    ReorderLevel = product.ReorderLevel,
                    Status = status,
                    TotalValue = product.Price * product.StockQuantity,
                    LastMovement = lastMovement?.MovementDate ?? DateTime.MinValue
                };

                report.Add(reportItem);
            }

            return ApiResponse<List<InventoryReportDto>>.SuccessResponse(report, "Inventory report retrieved");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting inventory report: {ex.Message}");
            return ApiResponse<List<InventoryReportDto>>.ErrorResponse($"Error retrieving report: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<InventoryReportDto>>> GetLowStockItemsAsync()
    {
        try
        {
            var report = await GetInventoryReportAsync();
            if (!report.Success)
            {
                return report;
            }

            var lowStockItems = report.Data!.Where(i => i.Status == "Low" || i.Status == "Critical").ToList();

            return ApiResponse<List<InventoryReportDto>>.SuccessResponse(
                lowStockItems, "Low stock items retrieved");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting low stock items: {ex.Message}");
            return ApiResponse<List<InventoryReportDto>>.ErrorResponse($"Error retrieving low stock items: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<InventoryReportDto>>> GetOutOfStockItemsAsync()
    {
        try
        {
            var products = await _unitOfWork.Products.FindAsync(p => p.StockQuantity == 0);
            var report = new List<InventoryReportDto>();

            foreach (var product in products)
            {
                report.Add(new InventoryReportDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    SKU = product.SKU,
                    CurrentStock = 0,
                    ReorderLevel = product.ReorderLevel,
                    Status = "Out of Stock",
                    TotalValue = 0
                });
            }

            return ApiResponse<List<InventoryReportDto>>.SuccessResponse(report, "Out of stock items retrieved");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting out of stock items: {ex.Message}");
            return ApiResponse<List<InventoryReportDto>>.ErrorResponse($"Error retrieving out of stock items: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<StockMovementAnalysisDto>>> GetStockMovementAnalysisAsync(
        DateTime startDate, DateTime endDate)
    {
        try
        {
            var movements = await _unitOfWork.Inventories.FindAsync(
                i => i.MovementDate >= startDate && i.MovementDate <= endDate);

            var products = await _unitOfWork.Products.GetAllAsync();

            var analysis = new List<StockMovementAnalysisDto>();

            foreach (var product in products)
            {
                var productMovements = movements.Where(m => m.ProductId == product.Id).ToList();

                if (productMovements.Any())
                {
                    var totalInbound = productMovements
                        .Where(m => m.MovementType == "In")
                        .Sum(m => m.Quantity);

                    var totalOutbound = productMovements
                        .Where(m => m.MovementType == "Out")
                        .Sum(m => m.Quantity);

                    analysis.Add(new StockMovementAnalysisDto
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        TotalInbound = totalInbound,
                        TotalOutbound = totalOutbound,
                        NetMovement = totalInbound - totalOutbound,
                        PeriodStart = startDate,
                        PeriodEnd = endDate
                    });
                }
            }

            return ApiResponse<List<StockMovementAnalysisDto>>.SuccessResponse(
                analysis, "Stock movement analysis retrieved");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting stock movement analysis: {ex.Message}");
            return ApiResponse<List<StockMovementAnalysisDto>>.ErrorResponse(
                $"Error retrieving analysis: {ex.Message}");
        }
    }

    public async Task<ApiResponse<decimal>> GetInventoryValueAsync()
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var totalValue = products.Sum(p => p.Price * p.StockQuantity);

            return ApiResponse<decimal>.SuccessResponse(totalValue, "Inventory value calculated");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating inventory value: {ex.Message}");
            return ApiResponse<decimal>.ErrorResponse($"Error calculating value: {ex.Message}");
        }
    }

    public async Task<ApiResponse<InventoryAuditDto>> PerformInventoryAuditAsync(
        int productId, int physicalCount, string reason)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
            {
                return ApiResponse<InventoryAuditDto>.ErrorResponse($"Product with id {productId} not found");
            }

            var variance = physicalCount - product.StockQuantity;

            // Record adjustment if there's a variance
            if (variance != 0)
            {
                var adjustmentDto = new CreateInventoryMovementDto
                {
                    ProductId = productId,
                    Quantity = physicalCount,
                    MovementType = "Adjustment",
                    Reference = $"Audit-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    Notes = $"Audit adjustment. Reason: {reason}"
                };

                await RecordMovementAsync(adjustmentDto, "system");
            }

            var auditRecord = new InventoryAuditDto
            {
                ProductId = productId,
                ProductName = product.Name,
                SystemStock = product.StockQuantity,
                PhysicalCount = physicalCount,
                Variance = variance,
                VarianceReason = reason,
                IsResolved = variance == 0,
                AuditDate = DateTime.UtcNow
            };

            _logger.LogInformation(
                $"Inventory audit completed for product {productId}. Variance: {variance}");

            return ApiResponse<InventoryAuditDto>.SuccessResponse(auditRecord, "Inventory audit completed");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error performing inventory audit: {ex.Message}");
            return ApiResponse<InventoryAuditDto>.ErrorResponse($"Error performing audit: {ex.Message}");
        }
    }

    private string GetStockStatus(int currentStock, int reorderLevel)
    {
        if (currentStock == 0)
            return "Out of Stock";
        if (currentStock <= reorderLevel / 2)
            return "Critical";
        if (currentStock <= reorderLevel)
            return "Low";
        if (currentStock > reorderLevel * 3)
            return "Excess";
        return "Normal";
    }
}
