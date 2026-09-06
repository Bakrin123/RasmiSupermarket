# Inventory Management Guide

## Overview

The RasmiSupermarket inventory management system provides comprehensive tools for tracking stock levels, recording movements, analyzing trends, and maintaining inventory accuracy.

## Features

### 1. Stock Movement Recording

Track all inventory movements with detailed logging:

- **Inbound (In)**: Stock received from suppliers
- **Outbound (Out)**: Stock sold to customers
- **Adjustment**: Manual stock corrections

#### Record Movement Endpoint

```http
POST /api/inventory/movement
Content-Type: application/json

{
  "productId": 1,
  "quantity": 50,
  "movementType": "In",
  "reference": "PO-2026-001",
  "notes": "Stock received from supplier ABC"
}
```

**Response:**
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
    "reference": "PO-2026-001",
    "notes": "Stock received from supplier ABC",
    "movementDate": "2026-09-06T22:00:00Z",
    "createdBy": "user123",
    "createdAt": "2026-09-06T22:00:00Z"
  }
}
```

### 2. Inventory History

View complete movement history for any product:

```http
GET /api/inventory/history/{productId}
```

**Response:**
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
      "reference": "PO-2026-001",
      "movementDate": "2026-09-06T22:00:00Z",
      "createdBy": "warehouse",
      "createdAt": "2026-09-06T22:00:00Z"
    },
    {
      "id": 2,
      "productId": 1,
      "productName": "Milk 1L",
      "quantity": 10,
      "movementType": "Out",
      "reference": "ORD-2026-001",
      "movementDate": "2026-09-06T23:00:00Z",
      "createdBy": "cashier1",
      "createdAt": "2026-09-06T23:00:00Z"
    }
  ]
}
```

### 3. Inventory Status Reports

#### Full Inventory Report

```http
GET /api/inventory/report
```

Shows status of all items:
- Current stock levels
- Reorder levels
- Stock status (Critical, Low, Normal, Excess, Out of Stock)
- Total inventory value

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

#### Low Stock Items

```http
GET /api/inventory/low-stock
```

Filters items with:
- Stock ≤ Reorder Level: **Low**
- Stock ≤ Reorder Level / 2: **Critical**

#### Out of Stock Items

```http
GET /api/inventory/out-of-stock
```

Shows all items with zero quantity.

### 4. Stock Movement Analysis

Analyze inventory movements over a period:

```http
POST /api/inventory/movement-analysis?startDate=2026-09-01&endDate=2026-09-30
```

**Response:**
```json
{
  "success": true,
  "message": "Stock movement analysis retrieved",
  "data": [
    {
      "productId": 1,
      "productName": "Milk 1L",
      "totalInbound": 200,
      "totalOutbound": 150,
      "netMovement": 50,
      "periodStart": "2026-09-01T00:00:00Z",
      "periodEnd": "2026-09-30T23:59:59Z"
    }
  ]
}
```

### 5. Inventory Value

Calculate total inventory value:

```http
GET /api/inventory/total-value
```

**Response:**
```json
{
  "success": true,
  "message": "Inventory value calculated",
  "data": 5250.75
}
```

Value = Sum of (Product Price × Current Stock Quantity)

### 6. Inventory Audit

Perform physical stock count and reconcile with system:

```http
POST /api/inventory/audit?productId=1&physicalCount=45&reason=Physical+count+completed
```

**Response:**
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
    "varianceReason": "Physical count completed",
    "isResolved": false,
    "auditDate": "2026-09-06T22:30:00Z"
  }
}
```

## Stock Status Definitions

| Status | Condition | Action |
|--------|-----------|--------|
| **Out of Stock** | Quantity = 0 | Urgent reorder required |
| **Critical** | Quantity ≤ Reorder Level / 2 | Immediate reorder |
| **Low** | Quantity ≤ Reorder Level | Plan to reorder |
| **Normal** | Reorder Level < Quantity ≤ Reorder Level × 3 | Monitor |
| **Excess** | Quantity > Reorder Level × 3 | Consider reducing orders |

## Use Cases

### Scenario 1: Receiving Stock from Supplier

1. Goods arrive from supplier
2. Warehouse staff counts items
3. Call movement endpoint:
   ```bash
   POST /api/inventory/movement
   {
     "productId": 5,
     "quantity": 100,
     "movementType": "In",
     "reference": "PO-2026-0542",
     "notes": "Delivery from ABC Suppliers"
   }
   ```
4. Product stock automatically increases
5. Movement is logged for audit trail

### Scenario 2: Processing Customer Sale

1. Customer purchases product
2. System records order
3. Call movement endpoint:
   ```bash
   POST /api/inventory/movement
   {
     "productId": 5,
     "quantity": 2,
     "movementType": "Out",
     "reference": "ORD-2026-0142",
     "notes": "Customer order"
   }
   ```
4. Stock reduces automatically
5. If stock becomes low, alert is triggered

### Scenario 3: Inventory Discrepancy

1. Manager performs physical count
2. Count shows 48 items but system shows 50
3. Call audit endpoint:
   ```bash
   POST /api/inventory/audit?productId=5&physicalCount=48&reason=Breakage+discovered
   ```
4. System records variance of -2
5. Adjustment movement created automatically
6. Stock is corrected to 48
7. Variance reason is documented

### Scenario 4: Monthly Analysis

1. Manager wants to analyze September sales
2. Call movement analysis:
   ```bash
   POST /api/inventory/movement-analysis?startDate=2026-09-01&endDate=2026-09-30
   ```
3. Receives breakdown of all inbound/outbound movements
4. Identifies fast-moving vs slow-moving products

## Best Practices

### 1. Regular Movement Recording
- Record all movements immediately
- Use meaningful references (PO numbers, Order numbers)
- Include detailed notes for traceability

### 2. Stock Reconciliation
- Perform physical counts periodically (weekly/monthly)
- Investigate variances promptly
- Document reasons for discrepancies

### 3. Alert Management
- Monitor low-stock alerts daily
- Set appropriate reorder levels based on demand
- Coordinate with procurement team for timely restocking

### 4. Analysis and Planning
- Review movement analysis monthly
- Identify seasonal trends
- Plan inventory levels accordingly

### 5. Safety Stock
- Maintain minimum stock levels for critical items
- Account for supplier lead times
- Factor in demand variability

## Data Retention

All inventory movements are permanently stored for:
- Audit trail
- Historical analysis
- Compliance reporting
- Financial valuation

## Performance Tips

1. **Use Batch Operations**: Record multiple movements in sequence for efficiency
2. **Archive Old Records**: For performance, consider archiving movements older than 2 years
3. **Index Key Fields**: Database is optimized for common queries (productId, movementDate)
4. **Cache Reports**: Dashboard summaries can be cached and refreshed hourly

## Troubleshooting

### Issue: Cannot record outbound movement
**Cause**: Insufficient stock
**Solution**: Check available quantity before processing sale. System prevents overselling.

### Issue: Audit variance not matching
**Cause**: Unrecorded movements since last count
**Solution**: Check inventory history for recent movements. Record any missing adjustments.

### Issue: Inventory value seems incorrect
**Cause**: Outdated product prices
**Solution**: Update product prices. Value is calculated using current prices.

## API Error Codes

| Code | Message | Resolution |
|------|---------|------------|
| 404 | Product not found | Verify product ID |
| 400 | Invalid movement type | Use: In, Out, or Adjustment |
| 400 | Insufficient stock | Check available quantity |
| 500 | Database error | Contact support |

---

**Inventory Module Version**: 1.0.0  
**Last Updated**: 2026-09-06
