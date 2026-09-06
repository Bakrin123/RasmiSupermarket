using RasmiSupermarket.Core.Entities;

namespace RasmiSupermarket.Data.Repositories;

/// <summary>
/// Unit of Work pattern interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Supplier> Suppliers { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderItem> OrderItems { get; }
    IRepository<Inventory> Inventories { get; }
    IRepository<User> Users { get; }
    IRepository<AuditLog> AuditLogs { get; }
    Task SaveChangesAsync();
}
