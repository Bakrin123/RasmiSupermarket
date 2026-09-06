using AutoMapper;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Entities;
using RasmiSupermarket.Core.Responses;
using RasmiSupermarket.Data.Repositories;

namespace RasmiSupermarket.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<CustomerDto>> GetCustomerByIdAsync(int id)
    {
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning($"Customer with id {id} not found");
                return ApiResponse<CustomerDto>.ErrorResponse($"Customer with id {id} not found");
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return ApiResponse<CustomerDto>.SuccessResponse(customerDto, "Customer retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting customer: {ex.Message}");
            return ApiResponse<CustomerDto>.ErrorResponse($"Error getting customer: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<CustomerDto>>> GetAllCustomersAsync()
    {
        try
        {
            var customers = await _unitOfWork.Customers.FindAsync(c => c.IsActive);
            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
            return ApiResponse<List<CustomerDto>>.SuccessResponse(customerDtos, "Customers retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting all customers: {ex.Message}");
            return ApiResponse<List<CustomerDto>>.ErrorResponse($"Error getting customers: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CreateCustomerDto dto)
    {
        try
        {
            var customer = _mapper.Map<Customer>(dto);
            customer.CreatedAt = DateTime.UtcNow;
            customer.IsActive = true;
            
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<CustomerDto>(customer);
            return ApiResponse<CustomerDto>.SuccessResponse(resultDto, "Customer created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating customer: {ex.Message}");
            return ApiResponse<CustomerDto>.ErrorResponse($"Error creating customer: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
    {
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return ApiResponse<CustomerDto>.ErrorResponse($"Customer with id {id} not found");
            }

            _mapper.Map(dto, customer);
            customer.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<CustomerDto>(customer);
            return ApiResponse<CustomerDto>.SuccessResponse(resultDto, "Customer updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating customer: {ex.Message}");
            return ApiResponse<CustomerDto>.ErrorResponse($"Error updating customer: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteCustomerAsync(int id)
    {
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return ApiResponse<bool>.ErrorResponse($"Customer with id {id} not found");
            }

            customer.IsActive = false;
            customer.UpdatedAt = DateTime.UtcNow;
            
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Customer deactivated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting customer: {ex.Message}");
            return ApiResponse<bool>.ErrorResponse($"Error deleting customer: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> AddLoyaltyPointsAsync(int customerId, decimal points)
    {
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
            {
                return ApiResponse<bool>.ErrorResponse($"Customer with id {customerId} not found");
            }

            customer.LoyaltyPoints += points;
            customer.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Loyalty points added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding loyalty points: {ex.Message}");
            return ApiResponse<bool>.ErrorResponse($"Error adding loyalty points: {ex.Message}");
        }
    }
}
