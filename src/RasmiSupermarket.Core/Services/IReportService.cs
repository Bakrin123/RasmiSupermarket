using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.Core.Services;

public interface IReportService
{
    Task<ApiResponse<SalesReportDto>> GetSalesReportAsync(DateTime startDate, DateTime endDate);
    Task<ApiResponse<List<CustomerReportDto>>> GetCustomerReportAsync();
    Task<ApiResponse<List<CategoryReportDto>>> GetCategoryReportAsync();
    Task<ApiResponse<DashboardSummaryDto>> GetDashboardSummaryAsync();
    Task<ApiResponse<List<ProductSalesDto>>> GetTopProductsAsync(int limit = 10);
    Task<ApiResponse<List<CustomerReportDto>>> GetTopCustomersAsync(int limit = 10);
    Task<ApiResponse<object>> ExportSalesReportAsync(DateTime startDate, DateTime endDate);
}
