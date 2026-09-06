# Reporting and Analytics Guide

## Overview

The RasmiSupermarket reporting system provides comprehensive analytics and insights into sales, inventory, customers, and business performance.

## Report Types

### 1. Sales Report

Detailed sales analysis for any date range:

```http
GET /api/reports/sales?startDate=2026-09-01&endDate=2026-09-30
```

**Response:**
```json
{
  "success": true,
  "message": "Sales report retrieved successfully",
  "data": {
    "totalOrders": 150,
    "totalRevenue": 15750.50,
    "averageOrderValue": 105.00,
    "totalItemsSold": 425,
    "dailySales": [
      {
        "date": "2026-09-01T00:00:00Z",
        "orderCount": 5,
        "revenue": 525.00,
        "averageOrderValue": 105.00
      },
      {
        "date": "2026-09-02T00:00:00Z",
        "orderCount": 4,
        "revenue": 420.00,
        "averageOrderValue": 105.00
      }
    ],
    "topProducts": [
      {
        "productId": 1,
        "productName": "Milk 1L",
        "sku": "MILK001",
        "quantitySold": 150,
        "totalRevenue": 375.00,
        "averagePrice": 2.50
      }
    ]
  }
}
```

**Metrics Provided:**
- Total orders in period
- Total revenue generated
- Average order value
- Total items sold
- Daily breakdown
- Top performing products

### 2. Customer Report

Comprehensive customer analytics:

```http
GET /api/reports/customers
```

**Response:**
```json
{
  "success": true,
  "message": "Customer report retrieved successfully",
  "data": [
    {
      "customerId": 1,
      "customerName": "Ahmed Hassan",
      "totalOrders": 12,
      "totalSpent": 1260.00,
      "averageOrderValue": 105.00,
      "lastPurchaseDate": "2026-09-06T15:30:00Z",
      "loyaltyPoints": 1260
    }
  ]
}
```

**Insights:**
- Customer purchase frequency
- Customer spending patterns
- Loyalty points balance
- Last activity date
- Average order value per customer

### 3. Category Report

Analyze performance by product category:

```http
GET /api/reports/categories
```

**Response:**
```json
{
  "success": true,
  "message": "Category report retrieved successfully",
  "data": [
    {
      "category": "Dairy",
      "totalProducts": 15,
      "totalItemsSold": 450,
      "totalRevenue": 1125.00,
      "averagePrice": 2.50
    },
    {
      "category": "Bakery",
      "totalProducts": 10,
      "totalItemsSold": 200,
      "totalRevenue": 600.00,
      "averagePrice": 3.00
    }
  ]
}
```

**Analysis:**
- Products per category
- Sales volume by category
- Revenue contribution
- Average pricing

### 4. Dashboard Summary

Quick overview of business metrics:

```http
GET /api/reports/dashboard-summary
```

**Response:**
```json
{
  "success": true,
  "message": "Dashboard summary retrieved successfully",
  "data": {
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
}
```

**KPIs:**
- Total products in catalog
- Active customer base
- Stock status summary
- Inventory valuation
- Today's performance
- Monthly performance

### 5. Top Products

Identify best-selling products:

```http
GET /api/reports/top-products?limit=10
```

**Response:**
```json
{
  "success": true,
  "message": "Top products retrieved successfully",
  "data": [
    {
      "productId": 1,
      "productName": "Milk 1L",
      "sku": "MILK001",
      "quantitySold": 450,
      "totalRevenue": 1125.00,
      "averagePrice": 2.50
    },
    {
      "productId": 3,
      "productName": "Bread",
      "sku": "BREAD001",
      "quantitySold": 200,
      "totalRevenue": 600.00,
      "averagePrice": 3.00
    }
  ]
}
```

**Use Cases:**
- Inventory focus and stocking decisions
- Promotional opportunities
- Supplier negotiations
- Product portfolio optimization

### 6. Top Customers

Identify high-value customers:

```http
GET /api/reports/top-customers?limit=10
```

**Response:**
```json
{
  "success": true,
  "message": "Top customers retrieved successfully",
  "data": [
    {
      "customerId": 5,
      "customerName": "Fatima Ali",
      "totalOrders": 48,
      "totalSpent": 5040.00,
      "averageOrderValue": 105.00,
      "lastPurchaseDate": "2026-09-06T17:45:00Z",
      "loyaltyPoints": 5040
    }
  ]
}
```

**Value:**
- VIP customer identification
- Retention strategy development
- Loyalty program targeting
- Special offer opportunities

### 7. Export Reports

Export sales data for external analysis:

```http
GET /api/reports/sales-export?startDate=2026-09-01&endDate=2026-09-30
```

**Response Format:** JSON (can be enhanced with CSV/Excel)

## Dashboard Widgets

### Real-time Metrics

| Widget | Metric | Refresh Rate |
|--------|--------|---------------|
| Sales Today | Revenue generated today | Real-time |
| Orders Today | Number of orders | Real-time |
| Low Stock | Count of items | Hourly |
| Out of Stock | Count of items | Hourly |
| Inventory Value | Total value | Hourly |
| Monthly Revenue | YTD revenue | Daily |
| Top Product | Best seller | Daily |
| Top Customer | Highest spender | Daily |

## Analysis Scenarios

### Scenario 1: Daily Operations Review

1. Manager logs in at 9 AM
2. Views dashboard summary
3. Checks for out-of-stock items
4. Verifies today's sales performance
5. Reviews low-stock alerts

```bash
GET /api/reports/dashboard-summary
GET /api/inventory/out-of-stock
GET /api/inventory/low-stock
GET /api/reports/sales?startDate=TODAY&endDate=TODAY
```

### Scenario 2: Weekly Performance Review

1. Manager reviews week's sales
2. Analyzes by product category
3. Checks customer acquisition
4. Reviews inventory movements

```bash
GET /api/reports/sales?startDate=WEEK_START&endDate=TODAY
GET /api/reports/categories
GET /api/reports/customers
POST /api/inventory/movement-analysis?startDate=WEEK_START&endDate=TODAY
```

### Scenario 3: Monthly Strategic Planning

1. Director reviews monthly performance
2. Analyzes revenue trends
3. Identifies top/bottom products
4. Plans inventory for next month
5. Sets sales targets

```bash
GET /api/reports/sales?startDate=MONTH_START&endDate=TODAY
GET /api/reports/top-products?limit=20
GET /api/reports/top-customers?limit=20
GET /api/inventory/report
GET /api/reports/dashboard-summary
```

## KPI Definitions

### Revenue Metrics

- **Total Revenue**: Sum of all order amounts
- **Daily Average**: Total Revenue / Number of Days
- **Monthly Revenue**: Revenue for current month
- **Average Order Value**: Total Revenue / Total Orders

### Customer Metrics

- **Total Customers**: Count of active customers
- **Customer Retention**: Repeat purchase rate
- **Lifetime Value**: Total spending by customer
- **Acquisition Cost**: (Marketing Spend / New Customers)

### Inventory Metrics

- **Inventory Turnover**: COGS / Average Inventory Value
- **Stock-out Rate**: Percentage of time product is unavailable
- **Inventory Accuracy**: Physical Count / System Count
- **Low Stock Items**: Count of items below reorder level

### Sales Metrics

- **Orders per Day**: Total Orders / Days in Period
- **Items per Order**: Total Items Sold / Total Orders
- **Category Performance**: Revenue / Category
- **Top Performer**: Highest revenue product

## Report Scheduling

### Recommended Report Schedule

| Report | Frequency | Recipient | Use |
|--------|-----------|-----------|-----|
| Dashboard Summary | Daily | Manager | Daily check-in |
| Sales Report | Daily | Management | Performance tracking |
| Inventory Report | Daily | Warehouse | Stock planning |
| Customer Report | Weekly | Sales Team | Retention focus |
| Category Report | Weekly | Category Manager | Performance review |
| Top Products | Weekly | Procurement | Ordering |
| Low Stock | Real-time | Warehouse | Urgent reorder |
| Monthly Summary | Monthly | Director | Strategic review |

## Data Export

All reports can be exported for:
- External analysis (Excel, Power BI)
- Stakeholder presentations
- Compliance reporting
- Historical record keeping

**Export Format**: JSON (can be transformed to CSV/Excel)

## Performance Optimization

### Caching Strategy

- Dashboard summaries: Cache 1 hour
- Category reports: Cache 1 hour
- Sales reports: Cache 15 minutes
- Inventory reports: Cache 30 minutes

### Query Optimization

- All reports use indexed queries
- Date range filtering for efficiency
- Aggregation at database level
- Pagination support for large datasets

## Report Limitations and Considerations

1. **Date Range**: Maximum 1 year for detailed reports
2. **Historical Data**: Maintained indefinitely
3. **Real-time**: Dashboard updates within 5 minutes
4. **Archived Data**: Older than 2 years available on request
5. **Custom Reports**: Contact admin for specialized needs

## Troubleshooting

### Issue: Report shows zero values
**Cause**: Date range has no data
**Solution**: Check date filters. Ensure orders exist in period.

### Issue: Slow report generation
**Cause**: Large date range or slow database
**Solution**: Reduce date range. Enable caching.

### Issue: Revenue calculations incorrect
**Cause**: Cancelled orders not excluded
**Solution**: Only "Completed" orders are included.

## Future Enhancements

- [ ] Predictive analytics
- [ ] Custom report builder
- [ ] Automated scheduled reports
- [ ] PDF/Excel export
- [ ] Real-time dashboard charts
- [ ] Customer segmentation analysis
- [ ] Forecasting tools
- [ ] Comparative analysis (YoY, MoM)

---

**Reporting Module Version**: 1.0.0  
**Last Updated**: 2026-09-06
