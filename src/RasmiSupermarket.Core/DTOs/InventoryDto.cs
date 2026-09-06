namespace RasmiSupermarket.Core.DTOs;

public class InventoryDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime MovementDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateInventoryMovementDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty; // In, Out, Adjustment
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class InventoryReportDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int ReorderLevel { get; set; }
    public string Status { get; set; } = string.Empty; // Critical, Low, Normal, Excess
    public decimal TotalValue { get; set; }
    public DateTime LastMovement { get; set; }
}

public class StockMovementAnalysisDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalInbound { get; set; }
    public int TotalOutbound { get; set; }
    public int NetMovement { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

public class InventoryAuditDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int SystemStock { get; set; }
    public int PhysicalCount { get; set; }
    public int Variance { get; set; }
    public string VarianceReason { get; set; } = string.Empty;
    public bool IsResolved { get; set; }
    public DateTime AuditDate { get; set; }
}
