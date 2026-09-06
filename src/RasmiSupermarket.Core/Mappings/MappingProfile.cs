using AutoMapper;
using RasmiSupermarket.Core.DTOs;
using RasmiSupermarket.Core.Entities;

namespace RasmiSupermarket.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductDto>().ReverseMap();

        // Customer mappings
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Customer, CreateCustomerDto>().ReverseMap();
        CreateMap<Customer, UpdateCustomerDto>().ReverseMap();

        // Supplier mappings
        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<Supplier, CreateSupplierDto>().ReverseMap();
        CreateMap<Supplier, UpdateSupplierDto>().ReverseMap();

        // Order mappings
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();

        // User mappings
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ForMember(dest => dest.Password, opt => opt.Ignore()).ReverseMap();
    }
}
