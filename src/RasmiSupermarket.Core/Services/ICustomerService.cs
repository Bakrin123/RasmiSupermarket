using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Responses;

namespace RasmiSupermarket.Core.Services;

public interface ICustomerService
{
    Task<ApiResponse<CustomerDto>> GetCustomerByIdAsync(int id);
    Task<ApiResponse<List<CustomerDto>>> GetAllCustomersAsync();
    Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CreateCustomerDto dto);
    Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerDto dto);
    Task<ApiResponse<bool>> DeleteCustomerAsync(int id);
    Task<ApiResponse<bool>> AddLoyaltyPointsAsync(int customerId, decimal points);
}
