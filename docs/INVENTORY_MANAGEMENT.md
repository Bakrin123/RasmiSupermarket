# Inventory Management Guide - RasmiSupermarket

## Overview

The Inventory Management module provides comprehensive stock tracking, movement recording, and inventory auditing capabilities.

## Key Features

### 1. Inventory Movement Recording

Track all stock movements with three types:
- **In**: Stock received from suppliers
- **Out**: Stock sold or used
- **Adjustment**: Manual stock corrections

#### API Endpoint
```
POST /api/inventory/movement
Content-Type: application/json

{
  "productId": 1,
  "quantity": 50,
  "movementType": "In",
  "reference": "PO-001",
  "notes": "Purchase order from ABC Supplies"
}
```

#### Response
```json
{
  "success": true,
  "message": "Inventory movement recorded successfully",
  "data": {
    "id": 1,
    "productId": 1,
    "productName": "Milk 1L",
    "quantity": 50,
    "movementType": "In",
    "reference": "PO-001",
    "notes": "Purchase order from ABC Supplies",
    "movementDate": "2026-09-06T22:46:24Z",
    "createdBy": "user123",
    "createdAt": "2026-09-06T22:46:24Z"
  }
}
```

### 2. Inventory History

View complete movement history for any product.

#### API Endpoint
```
GET /api/inventory/history/{productId}
```

#### Response
```json
{
  "success": true,
  "message": "Inventory history retrieved",
  "data": [
    {
      "id": 1,
      "productId": 1,
      "productName": "Milk 1L",
      "quantity": 50,
      "movementType": "In",
      "movementDate": "2026-09-06T22:46:24Z"
    },
    {
      "id": 2,
      "productId": 1,
      "productName": "Milk 1L",
      "quantity": 10,
      "movementType": "Out",
      "movementDate": "2026-09-06T23:00:00Z"
    }
  ]
}
```

### 3. Inventory Report

Comprehensive stock status overview.

#### API Endpoint
```
GET /api/inventory/report
```

#### Response
```json
{
  "success": true,
  "message": "Inventory report retrieved",
  "data": [
    {
      "productId": 1,
      "productName": "Milk 1L",
      "sku": "MILK001",
      "currentStock": 40,
      "reorderLevel": 10,
      "status": "Normal",
      "totalValue": 100.00,
      "lastMovement": "2026-09-06T23:00:00Z"
    }
  ]
}
```

#### Stock Status Values
- **Out of Stock**: 0 units
- **Critical**: ≤ 50% of reorder level
- **Low**: ≤ reorder level
- **Normal**: > reorder level and ≤ 3x reorder level
- **Excess**: > 3x reorder level

### 4. Low Stock Items

Get all products below reorder level.

#### API Endpoint
```
GET /api/inventory/low-stock
```

### 5. Out of Stock Items

Get all products with zero stock.

#### API Endpoint
```
GET /api/inventory/out-of-stock
```

### 6. Total Inventory Value

Calculate total monetary value of all inventory.

#### API Endpoint
```
GET /api/inventory/total-value
```

#### Response
```json
{
  "success": true,
  "message": "Inventory value calculated",
  "data": 15000.50
}
```

### 7. Stock Movement Analysis

Analyze stock movements over a period.

#### API Endpoint
```
POST /api/inventory/movement-analysis
?startDate=2026-09-01&endDate=2026-09-30
```

#### Response
```json
{
  "success": true,
  "message": "Stock movement analysis retrieved",
  "data": [
    {
      "productId": 1,
      "productName": "Milk 1L",
      "totalInbound": 100,
      "totalOutbound": 60,
      "netMovement": 40,
      "periodStart": "2026-09-01T00:00:00Z",
      "periodEnd": "2026-09-30T23:59:59Z"
    }
  ]
}
```

### 8. Inventory Audit

Perform physical inventory count and reconciliation.

#### API Endpoint
```
POST /api/inventory/audit
?productId=1&physicalCount=45&reason=Monthly+cycle+count
```

#### Response
```json
{
  "success": true,
  "message": "Inventory audit completed",
  "data": {
    "productId": 1,
    "productName": "Milk 1L",
    "systemStock": 40,
    "physicalCount": 45,
    "variance": 5,
    "varianceReason": "Monthly cycle count",
    "isResolved": false,
    "auditDate": "2026-09-06T22:46:24Z"
  }
}
```

## Best Practices

### Recording Movements
1. Always include descriptive references (PO number, Order ID, etc.)
2. Add notes explaining the movement reason
3. Record movements immediately to maintain accuracy
4. Use the correct movement type for proper stock calculation

### Stock Management
1. Regularly review low stock items
2. Set appropriate reorder levels based on sales velocity
3. Perform periodic physical counts (cycle counts)
4. Investigate and resolve variances promptly
5. Monitor inventory turnover rates

### Audit Process
1. Schedule regular cycle counts (daily, weekly, monthly)
2. Record physical counts accurately
3. Document variance reasons
4. Create adjustment movements to resolve discrepancies
5. Keep audit trail for compliance

## Common Issues and Solutions

### Issue: Insufficient Stock Error
**Cause**: Attempting to record more stock outbound than available
**Solution**: Record an inbound movement first or check system vs. physical stock

### Issue: Negative Stock
**Cause**: Out movements without proper validation
**Solution**: Use inventory audit to correct with adjustment movement

### Issue: Large Variances
**Cause**: System stock doesn't match physical count
**Solution**: 
1. Investigate root cause (theft, damage, data entry error)
2. Review recent movements
3. Perform detailed audit
4. Record adjustment movement with documentation

## Reporting Features

The Inventory Management system integrates with reporting to provide:
- Inventory valuation reports
- Stock aging analysis
- Movement trends
- Reorder recommendations
- ABC inventory classification

---

**Version**: 1.0.0  
**Last Updated**: 2026-09-06
