using Microsoft.EntityFrameworkCore;
using RasmiSupermarket.Core.Entities;

namespace RasmiSupermarket.Data;

public class RasmiSupermarketContext : DbContext
{
    public RasmiSupermarketContext(DbContextOptions<RasmiSupermarketContext> options) 
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Inventory> Inventories { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product configuration
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<Product>()
            .Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique();

        // Customer configuration
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Customer>()
            .Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Customer>()
            .Property(c => c.Email)
            .HasMaxLength(255);

        // Supplier configuration
        modelBuilder.Entity<Supplier>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<Supplier>()
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<Supplier>()
            .Property(s => s.CreditLimit)
            .HasColumnType("decimal(18,2)");

        // Order configuration
        modelBuilder.Entity<Order>()
            .HasKey(o => o.Id);
        modelBuilder.Entity<Order>()
            .Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderNumber)
            .IsUnique();

        // OrderItem configuration
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.Id);
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.SubTotal)
            .HasColumnType("decimal(18,2)");

        // Inventory configuration
        modelBuilder.Entity<Inventory>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Inventory>()
            .Property(i => i.MovementType)
            .IsRequired()
            .HasMaxLength(50);

        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>()
            .HasKey(a => a.Id);
        modelBuilder.Entity<AuditLog>()
            .Property(a => a.EntityName)
            .IsRequired()
            .HasMaxLength(255);
    }
}
