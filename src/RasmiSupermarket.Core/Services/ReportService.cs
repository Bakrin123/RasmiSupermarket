using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Responses;
using RasmiSupermarket.Data.Repositories;

namespace RasmiSupermarket.Core.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReportService> _logger;

    public ReportService(IUnitOfWork unitOfWork, ILogger<ReportService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<SalesReportDto>> GetSalesReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var orders = await _unitOfWork.Orders.FindAsync(
                o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status == "Completed");

            var report = new SalesReportDto
            {
                TotalOrders = orders.Count,
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0,
                TotalItemsSold = 0, // Will be calculated from order items
                DailySales = new List<DailySalesDto>(),
                TopProducts = new List<ProductSalesDto>()
            };

            // Calculate daily sales
            var dailyGrouping = orders.GroupBy(o => o.OrderDate.Date);
            foreach (var day in dailyGrouping)
            {
                var dayOrders = day.ToList();
                report.DailySales.Add(new DailySalesDto
                {
                    Date = day.Key,
                    OrderCount = dayOrders.Count,
                    Revenue = dayOrders.Sum(o => o.TotalAmount),
                    AverageOrderValue = dayOrders.Average(o => o.TotalAmount)
                });
            }

            _logger.LogInformation($"Sales report generated for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            return ApiResponse<SalesReportDto>.SuccessResponse(report, "Sales report retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating sales report: {ex.Message}");
            return ApiResponse<SalesReportDto>.ErrorResponse($"Error generating report: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<CustomerReportDto>>> GetCustomerReportAsync()
    {
        try
        {
            var customers = await _unitOfWork.Customers.FindAsync(c => c.IsActive);
            var orders = await _unitOfWork.Orders.FindAsync(o => o.Status == "Completed");

            var report = new List<CustomerReportDto>();

            foreach (var customer in customers)
            {
                var customerOrders = orders.Where(o => o.CustomerId == customer.Id).ToList();

                var customerReport = new CustomerReportDto
                {
                    CustomerId = customer.Id,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    TotalOrders = customerOrders.Count,
                    TotalSpent = customerOrders.Sum(o => o.TotalAmount),
                    AverageOrderValue = customerOrders.Any() ? customerOrders.Average(o => o.TotalAmount) : 0,
                    LastPurchaseDate = customerOrders.Any() ? customerOrders.Max(o => o.OrderDate) : DateTime.MinValue,
                    LoyaltyPoints = customer.LoyaltyPoints
                };

                report.Add(customerReport);
            }

            return ApiResponse<List<CustomerReportDto>>.SuccessResponse(report, "Customer report retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating customer report: {ex.Message}");
            return ApiResponse<List<CustomerReportDto>>.ErrorResponse($"Error generating report: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<CategoryReportDto>>> GetCategoryReportAsync()
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var categories = products.GroupBy(p => p.Category).ToList();

            var report = new List<CategoryReportDto>();

            foreach (var category in categories)
            {
                var categoryReport = new CategoryReportDto
                {
                    Category = category.Key,
                    TotalProducts = category.Count(),
                    TotalItemsSold = 0, // Will be calculated from order items
                    TotalRevenue = category.Sum(p => p.Price * p.StockQuantity),
                    AveragePrice = category.Average(p => p.Price)
                };

                report.Add(categoryReport);
            }

            return ApiResponse<List<CategoryReportDto>>.SuccessResponse(report, "Category report retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating category report: {ex.Message}");
            return ApiResponse<List<CategoryReportDto>>.ErrorResponse($"Error generating report: {ex.Message}");
        }
    }

    public async Task<ApiResponse<DashboardSummaryDto>> GetDashboardSummaryAsync()
    {
        try
        {
            var allProducts = await _unitOfWork.Products.GetAllAsync();
            var allCustomers = await _unitOfWork.Customers.FindAsync(c => c.IsActive);
            var todayOrders = await _unitOfWork.Orders.FindAsync(
                o => o.OrderDate.Date == DateTime.UtcNow.Date && o.Status == "Completed");
            var monthlyOrders = await _unitOfWork.Orders.FindAsync(
                o => o.OrderDate.Month == DateTime.UtcNow.Month && 
                     o.OrderDate.Year == DateTime.UtcNow.Year &&
                     o.Status == "Completed");

            var lowStockProducts = allProducts.Count(p => p.StockQuantity <= p.ReorderLevel && p.StockQuantity > 0);
            var outOfStockProducts = allProducts.Count(p => p.StockQuantity == 0);

            var summary = new DashboardSummaryDto
            {
                TotalProducts = allProducts.Count,
                TotalCustomers = allCustomers.Count,
                LowStockProducts = lowStockProducts,
                OutOfStockProducts = outOfStockProducts,
                TotalInventoryValue = allProducts.Sum(p => p.Price * p.StockQuantity),
                TodayOrders = todayOrders.Count,
                TodayRevenue = todayOrders.Sum(o => o.TotalAmount),
                MonthlyRevenue = monthlyOrders.Sum(o => o.TotalAmount),
                MonthlyOrders = monthlyOrders.Count
            };

            _logger.LogInformation("Dashboard summary generated");

            return ApiResponse<DashboardSummaryDto>.SuccessResponse(summary, "Dashboard summary retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating dashboard summary: {ex.Message}");
            return ApiResponse<DashboardSummaryDto>.ErrorResponse($"Error generating summary: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<ProductSalesDto>>> GetTopProductsAsync(int limit = 10)
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsync();

            var topProducts = products
                .OrderByDescending(p => p.StockQuantity) // Using stock as proxy for sales (can be enhanced with actual sales data)
                .Take(limit)
                .Select(p => new ProductSalesDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    SKU = p.SKU,
                    QuantitySold = p.StockQuantity,
                    TotalRevenue = p.Price * p.StockQuantity,
                    AveragePrice = p.Price
                })
                .ToList();

            return ApiResponse<List<ProductSalesDto>>.SuccessResponse(topProducts, "Top products retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting top products: {ex.Message}");
            return ApiResponse<List<ProductSalesDto>>.ErrorResponse($"Error retrieving top products: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<CustomerReportDto>>> GetTopCustomersAsync(int limit = 10)
    {
        try
        {
            var customerReport = await GetCustomerReportAsync();
            if (!customerReport.Success)
            {
                return new ApiResponse<List<CustomerReportDto>>
                {
                    Success = false,
                    Message = customerReport.Message
                };
            }

            var topCustomers = customerReport.Data!
                .OrderByDescending(c => c.TotalSpent)
                .Take(limit)
                .ToList();

            return ApiResponse<List<CustomerReportDto>>.SuccessResponse(topCustomers, "Top customers retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting top customers: {ex.Message}");
            return ApiResponse<List<CustomerReportDto>>.ErrorResponse($"Error retrieving top customers: {ex.Message}");
        }
    }

    public async Task<ApiResponse<object>> ExportSalesReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var report = await GetSalesReportAsync(startDate, endDate);
            if (!report.Success)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = report.Message
                };
            }

            // This is a placeholder for export functionality
            // In production, you would generate CSV, Excel, or PDF
            var exportData = new
            {
                ReportType = "Sales Report",
                GeneratedDate = DateTime.UtcNow,
                PeriodStart = startDate,
                PeriodEnd = endDate,
                Data = report.Data
            };

            _logger.LogInformation($"Sales report exported for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            return ApiResponse<object>.SuccessResponse(exportData, "Report exported successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error exporting sales report: {ex.Message}");
            return ApiResponse<object>.ErrorResponse($"Error exporting report: {ex.Message}");
        }
    }
}
