namespace RasmiSupermarket.Core.Entities;

/// <summary>
/// Represents inventory stock movements
/// </summary>
public class Inventory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty; // In, Out, Adjustment
    public string Reference { get; set; } = string.Empty; // Order ID, Adjustment ID, etc.
    public string Notes { get; set; } = string.Empty;
    public DateTime MovementDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
