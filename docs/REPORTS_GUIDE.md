# Reporting Guide - RasmiSupermarket

## Overview

The Reporting module provides comprehensive business intelligence and analytics for sales, customers, inventory, and operations.

## Available Reports

### 1. Sales Report

Comprehensive sales analysis for a specified date range.

#### API Endpoint
```
GET /api/reports/sales
?startDate=2026-09-01&endDate=2026-09-30
```

#### Response
```json
{
  "success": true,
  "message": "Sales report retrieved successfully",
  "data": {
    "totalOrders": 150,
    "totalRevenue": 15000.50,
    "averageOrderValue": 100.00,
    "totalItemsSold": 500,
    "dailySales": [
      {
        "date": "2026-09-01",
        "orderCount": 5,
        "revenue": 500.00,
        "averageOrderValue": 100.00
      }
    ],
    "topProducts": [
      {
        "productId": 1,
        "productName": "Milk 1L",
        "sku": "MILK001",
        "quantitySold": 200,
        "totalRevenue": 500.00,
        "averagePrice": 2.50
      }
    ]
  }
}
```

#### Key Metrics
- **Total Orders**: Number of completed transactions
- **Total Revenue**: Sum of all order amounts
- **Average Order Value**: Mean transaction value
- **Daily Sales Breakdown**: Sales by date
- **Top Products**: Best-selling items

### 2. Customer Report

Detailed customer activity and spending analysis.

#### API Endpoint
```
GET /api/reports/customers
```

#### Response
```json
{
  "success": true,
  "message": "Customer report retrieved successfully",
  "data": [
    {
      "customerId": 1,
      "customerName": "Ahmed Hassan",
      "totalOrders": 25,
      "totalSpent": 2500.00,
      "averageOrderValue": 100.00,
      "lastPurchaseDate": "2026-09-06T15:30:00Z",
      "loyaltyPoints": 500
    }
  ]
}
```

#### Key Metrics
- **Total Orders**: Number of purchases
- **Total Spent**: Cumulative spending
- **Average Order Value**: Mean purchase amount
- **Last Purchase**: Most recent transaction date
- **Loyalty Points**: Accumulated rewards

### 3. Category Report

Sales and inventory analysis by product category.

#### API Endpoint
```
GET /api/reports/categories
```

#### Response
```json
{
  "success": true,
  "message": "Category report retrieved successfully",
  "data": [
    {
      "category": "Dairy",
      "totalProducts": 15,
      "totalItemsSold": 200,
      "totalRevenue": 500.00,
      "averagePrice": 2.50
    }
  ]
}
```

### 4. Dashboard Summary

Quick overview of key business metrics.

#### API Endpoint
```
GET /api/reports/dashboard-summary
```

#### Response
```json
{
  "success": true,
  "message": "Dashboard summary retrieved successfully",
  "data": {
    "totalProducts": 500,
    "totalCustomers": 1000,
    "lowStockProducts": 25,
    "outOfStockProducts": 5,
    "totalInventoryValue": 50000.00,
    "todayOrders": 10,
    "todayRevenue": 1000.00,
    "monthlyRevenue": 30000.00,
    "monthlyOrders": 300
  }
}
```

#### KPIs Tracked
- Product inventory status
- Customer base size
- Stock availability
- Inventory valuation
- Daily and monthly sales
- Order volume

### 5. Top Products

Identify best-performing products.

#### API Endpoint
```
GET /api/reports/top-products
?limit=10
```

#### Response
```json
{
  "success": true,
  "message": "Top products retrieved successfully",
  "data": [
    {
      "productId": 1,
      "productName": "Milk 1L",
      "sku": "MILK001",
      "quantitySold": 200,
      "totalRevenue": 500.00,
      "averagePrice": 2.50
    }
  ]
}
```

### 6. Top Customers

Identify most valuable customers.

#### API Endpoint
```
GET /api/reports/top-customers
?limit=10
```

#### Response
```json
{
  "success": true,
  "message": "Top customers retrieved successfully",
  "data": [
    {
      "customerId": 1,
      "customerName": "Ahmed Hassan",
      "totalOrders": 25,
      "totalSpent": 2500.00,
      "averageOrderValue": 100.00,
      "lastPurchaseDate": "2026-09-06T15:30:00Z",
      "loyaltyPoints": 500
    }
  ]
}
```

### 7. Sales Report Export

Export sales data for external analysis.

#### API Endpoint
```
GET /api/reports/sales-export
?startDate=2026-09-01&endDate=2026-09-30
```

#### Response
```json
{
  "success": true,
  "message": "Report exported successfully",
  "data": {
    "reportType": "Sales Report",
    "generatedDate": "2026-09-06T22:46:24Z",
    "periodStart": "2026-09-01",
    "periodEnd": "2026-09-30",
    "data": { /* Sales report data */ }
  }
}
```

## Report Usage Examples

### Daily Business Review

1. **Get Dashboard Summary**
   ```bash
   curl -X GET "https://localhost:5001/api/reports/dashboard-summary" \
     -H "Authorization: Bearer {token}"
   ```

2. **Check Sales for Today**
   ```bash
   curl -X GET "https://localhost:5001/api/reports/sales?startDate=2026-09-06&endDate=2026-09-06" \
     -H "Authorization: Bearer {token}"
   ```

### Weekly Performance Analysis

1. **Get Weekly Sales Report**
   ```bash
   curl -X GET "https://localhost:5001/api/reports/sales?startDate=2026-08-31&endDate=2026-09-06" \
     -H "Authorization: Bearer {token}"
   ```

2. **Identify Top Performers**
   ```bash
   curl -X GET "https://localhost:5001/api/reports/top-products?limit=5" \
     -H "Authorization: Bearer {token}"
   ```

### Monthly Business Review

1. **Get Complete Monthly Sales**
   ```bash
   curl -X GET "https://localhost:5001/api/reports/sales?startDate=2026-09-01&endDate=2026-09-30" \
     -H "Authorization: Bearer {token}"
   ```

2. **Analyze Customer Spending**
   ```bash
   curl -X GET "https://localhost:5001/api/reports/customers" \
     -H "Authorization: Bearer {token}"
   ```

3. **Review Category Performance**
   ```bash
   curl -X GET "https://localhost:5001/api/reports/categories" \
     -H "Authorization: Bearer {token}"
   ```

## Metrics and KPIs

### Sales Metrics
- **Revenue**: Total sales amount
- **Order Count**: Number of transactions
- **Average Order Value (AOV)**: Mean transaction size
- **Units Sold**: Total quantity of items
- **Revenue Growth**: Period-over-period comparison

### Customer Metrics
- **Customer Count**: Total active customers
- **Customer Lifetime Value (CLV)**: Total spending per customer
- **Repeat Purchase Rate**: Percentage of returning customers
- **Average Purchase Frequency**: Orders per customer per period
- **Customer Acquisition Cost (CAC)**: Cost to acquire new customers

### Inventory Metrics
- **Inventory Turnover**: How many times inventory sells and restocks
- **Stock-out Incidents**: Number of out-of-stock occurrences
- **Inventory Carrying Cost**: Cost to maintain inventory
- **Days Inventory Outstanding (DIO)**: Average days to sell inventory
- **Inventory Accuracy**: Physical count vs. system count

### Operational Metrics
- **Product Mix**: Sales distribution by category
- **Seasonal Trends**: Sales patterns over time
- **Peak Hours/Days**: High-traffic periods
- **Average Transaction Time**: Checkout speed

## Report Scheduling (Future Enhancement)

Planned features:
- Automated daily report generation
- Email report delivery
- Custom report builder
- Scheduled exports to cloud storage
- Real-time dashboard updates

## Data Visualization (Future Enhancement)

Planned features:
- Charts and graphs
- Interactive dashboards
- Trend analysis visualizations
- Heatmaps for peak periods
- Custom dashboard builder

## Performance Optimization

### Best Practices
1. Use specific date ranges to limit data volume
2. Run heavy reports during off-peak hours
3. Cache frequently accessed reports
4. Archive historical data periodically
5. Use indexes on report-critical columns

### Query Performance
- Sales reports optimized for fast aggregation
- Customer reports use efficient grouping
- Category analysis pre-calculated when possible
- Real-time dashboard uses cached snapshots

---

**Version**: 1.0.0  
**Last Updated**: 2026-09-06
