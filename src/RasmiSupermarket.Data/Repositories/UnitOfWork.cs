using RasmiSupermarket.Core.Entities;

namespace RasmiSupermarket.Data.Repositories;

/// <summary>
/// Unit of Work pattern implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly RasmiSupermarketContext _context;
    private IRepository<Product>? _products;
    private IRepository<Customer>? _customers;
    private IRepository<Supplier>? _suppliers;
    private IRepository<Order>? _orders;
    private IRepository<OrderItem>? _orderItems;
    private IRepository<Inventory>? _inventories;
    private IRepository<User>? _users;
    private IRepository<AuditLog>? _auditLogs;

    public UnitOfWork(RasmiSupermarketContext context)
    {
        _context = context;
    }

    public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
    public IRepository<Customer> Customers => _customers ??= new Repository<Customer>(_context);
    public IRepository<Supplier> Suppliers => _suppliers ??= new Repository<Supplier>(_context);
    public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);
    public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);
    public IRepository<Inventory> Inventories => _inventories ??= new Repository<Inventory>(_context);
    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<AuditLog> AuditLogs => _auditLogs ??= new Repository<AuditLog>(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
