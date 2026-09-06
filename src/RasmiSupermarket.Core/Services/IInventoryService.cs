using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.Core.Services;

public interface IInventoryService
{
    Task<ApiResponse<InventoryDto>> RecordMovementAsync(CreateInventoryMovementDto dto, string userId);
    Task<ApiResponse<List<InventoryDto>>> GetInventoryHistoryAsync(int productId);
    Task<ApiResponse<List<InventoryReportDto>>> GetInventoryReportAsync();
    Task<ApiResponse<List<InventoryReportDto>>> GetLowStockItemsAsync();
    Task<ApiResponse<List<InventoryReportDto>>> GetOutOfStockItemsAsync();
    Task<ApiResponse<List<StockMovementAnalysisDto>>> GetStockMovementAnalysisAsync(
        DateTime startDate, DateTime endDate);
    Task<ApiResponse<decimal>> GetInventoryValueAsync();
    Task<ApiResponse<InventoryAuditDto>> PerformInventoryAuditAsync(
        int productId, int physicalCount, string reason);
}
