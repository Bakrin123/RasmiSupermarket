namespace RasmiSupermarket.Core.Entities;

/// <summary>
/// Represents an item in an order
/// </summary>
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
