# Complete API Reference - RasmiSupermarket

## Inventory Management Endpoints

### Record Inventory Movement

```http
POST /api/inventory/movement
Content-Type: application/json

{
  "productId": 1,
  "quantity": 50,
  "movementType": "In",
  "reference": "PO-2026-001",
  "notes": "Stock received"
}

Response: 200 OK
{
  "success": true,
  "message": "Inventory movement recorded successfully",
  "data": { InventoryDto }
}
```

### Get Inventory History

```http
GET /api/inventory/history/{productId}

Response: 200 OK
[
  { InventoryDto },
  { InventoryDto }
]
```

### Get Inventory Report

```http
GET /api/inventory/report

Response: 200 OK
[
  { InventoryReportDto },
  { InventoryReportDto }
]
```

### Get Low Stock Items

```http
GET /api/inventory/low-stock

Response: 200 OK
[
  { InventoryReportDto }
]
```

### Get Out of Stock Items

```http
GET /api/inventory/out-of-stock

Response: 200 OK
[
  { InventoryReportDto }
]
```

### Get Total Inventory Value

```http
GET /api/inventory/total-value

Response: 200 OK
{
  "success": true,
  "message": "Inventory value calculated",
  "data": 25750.50
}
```

### Get Stock Movement Analysis

```http
POST /api/inventory/movement-analysis?startDate=2026-09-01&endDate=2026-09-30

Response: 200 OK
[
  { StockMovementAnalysisDto }
]
```

### Perform Inventory Audit

```http
POST /api/inventory/audit?productId=1&physicalCount=45&reason=Physical+count

Response: 200 OK
{
  "success": true,
  "message": "Inventory audit completed",
  "data": { InventoryAuditDto }
}
```

## Reporting Endpoints

### Get Sales Report

```http
GET /api/reports/sales?startDate=2026-09-01&endDate=2026-09-30

Response: 200 OK
{
  "success": true,
  "message": "Sales report retrieved successfully",
  "data": { SalesReportDto }
}
```

### Get Customer Report

```http
GET /api/reports/customers

Response: 200 OK
{
  "success": true,
  "message": "Customer report retrieved successfully",
  "data": [ { CustomerReportDto } ]
}
```

### Get Category Report

```http
GET /api/reports/categories

Response: 200 OK
{
  "success": true,
  "message": "Category report retrieved successfully",
  "data": [ { CategoryReportDto } ]
}
```

### Get Dashboard Summary

```http
GET /api/reports/dashboard-summary

Response: 200 OK
{
  "success": true,
  "message": "Dashboard summary retrieved successfully",
  "data": { DashboardSummaryDto }
}
```

### Get Top Products

```http
GET /api/reports/top-products?limit=10

Response: 200 OK
{
  "success": true,
  "message": "Top products retrieved successfully",
  "data": [ { ProductSalesDto } ]
}
```

### Get Top Customers

```http
GET /api/reports/top-customers?limit=10

Response: 200 OK
{
  "success": true,
  "message": "Top customers retrieved successfully",
  "data": [ { CustomerReportDto } ]
}
```

### Export Sales Report

```http
GET /api/reports/sales-export?startDate=2026-09-01&endDate=2026-09-30

Response: 200 OK
{
  "success": true,
  "message": "Report exported successfully",
  "data": { ExportedReportData }
}
```

## Data Transfer Objects (DTOs)

### InventoryDto
```json
{
  "id": 1,
  "productId": 1,
  "productName": "Milk 1L",
  "quantity": 50,
  "movementType": "In",
  "reference": "PO-2026-001",
  "notes": "Stock received",
  "movementDate": "2026-09-06T00:00:00Z",
  "createdBy": "warehouse",
  "createdAt": "2026-09-06T00:00:00Z"
}
```

### InventoryReportDto
```json
{
  "productId": 1,
  "productName": "Milk 1L",
  "sku": "MILK001",
  "currentStock": 100,
  "reorderLevel": 10,
  "status": "Normal",
  "totalValue": 250.00,
  "lastMovement": "2026-09-06T00:00:00Z"
}
```

### StockMovementAnalysisDto
```json
{
  "productId": 1,
  "productName": "Milk 1L",
  "totalInbound": 200,
  "totalOutbound": 150,
  "netMovement": 50,
  "periodStart": "2026-09-01T00:00:00Z",
  "periodEnd": "2026-09-30T23:59:59Z"
}
```

### InventoryAuditDto
```json
{
  "productId": 1,
  "productName": "Milk 1L",
  "systemStock": 100,
  "physicalCount": 98,
  "variance": -2,
  "varianceReason": "Damaged items",
  "isResolved": true,
  "auditDate": "2026-09-06T00:00:00Z"
}
```

### SalesReportDto
```json
{
  "totalOrders": 150,
  "totalRevenue": 15750.50,
  "averageOrderValue": 105.00,
  "totalItemsSold": 425,
  "dailySales": [ { DailySalesDto } ],
  "topProducts": [ { ProductSalesDto } ]
}
```

### DashboardSummaryDto
```json
{
  "totalProducts": 125,
  "totalCustomers": 450,
  "lowStockProducts": 8,
  "outOfStockProducts": 2,
  "totalInventoryValue": 25750.50,
  "todayOrders": 15,
  "todayRevenue": 1575.00,
  "monthlyRevenue": 45230.75,
  "monthlyOrders": 425
}
```

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "Invalid movement type",
  "data": null,
  "errors": ["error_code"],
  "timestamp": "2026-09-06T00:00:00Z"
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Product with id 999 not found",
  "data": null,
  "errors": ["not_found"],
  "timestamp": "2026-09-06T00:00:00Z"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An unexpected error occurred",
  "data": null,
  "errors": ["database_error"],
  "timestamp": "2026-09-06T00:00:00Z"
}
```

## Query Parameters

### Pagination
- `page`: Page number (default: 1)
- `pageSize`: Items per page (default: 10, max: 100)

### Filtering
- `startDate`: Filter by start date (format: YYYY-MM-DD)
- `endDate`: Filter by end date (format: YYYY-MM-DD)
- `limit`: Number of results (used in top products/customers)

### Sorting
- `sortBy`: Column name to sort
- `sortOrder`: asc or desc

## Authentication

All endpoints require JWT token:

```http
Authorization: Bearer <jwt_token>
```

## Rate Limiting

- 100 requests per minute per user
- 1000 requests per hour per user

## Versioning

Current API Version: 1.0.0

Future versions will be prefixed: `/api/v2/`

---

**API Reference Version**: 1.0.0  
**Last Updated**: 2026-09-06
