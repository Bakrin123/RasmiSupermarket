# API Documentation - RasmiSupermarket

## Authentication

All protected endpoints require a JWT token in the Authorization header:
```
Authorization: Bearer <token>
```

## Base URL
```
https://localhost:5001/api
```

## Endpoints

### Authentication

#### Login
```
POST /auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Password123!"
}

Response:
{
  "success": true,
  "message": "Login successful",
  "data": {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "role": "Admin"
  }
}
```

#### Register
```
POST /auth/register
Content-Type: application/json

{
  "username": "newuser",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "password": "Password123!",
  "role": "User"
}
```

### Products

#### Get All Products
```
GET /products
Authorization: Bearer <token>

Response:
{
  "success": true,
  "message": "Products retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "Milk 1L",
      "sku": "MILK001",
      "description": "Fresh dairy milk",
      "price": 2.50,
      "stockQuantity": 100,
      "reorderLevel": 10,
      "category": "Dairy",
      "barcode": "1234567890",
      "isActive": true,
      "createdAt": "2026-09-06T00:00:00Z"
    }
  ]
}
```

#### Get Product by ID
```
GET /products/{id}
Authorization: Bearer <token>
```

#### Get Products by Category
```
GET /products/category/{category}
Authorization: Bearer <token>
```

#### Get Low Stock Products
```
GET /products/low-stock
Authorization: Bearer <token>
```

#### Create Product
```
POST /products
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Bread",
  "sku": "BREAD001",
  "description": "Fresh bread",
  "price": 3.00,
  "stockQuantity": 50,
  "reorderLevel": 10,
  "category": "Bakery",
  "barcode": "0987654321",
  "isActive": true
}
```

#### Update Product
```
PUT /products/{id}
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Updated Name",
  "price": 3.50,
  ...
}
```

#### Delete Product
```
DELETE /products/{id}
Authorization: Bearer <token>
```

### Customers

#### Get All Customers
```
GET /customers
Authorization: Bearer <token>
```

#### Get Customer by ID
```
GET /customers/{id}
Authorization: Bearer <token>
```

#### Create Customer
```
POST /customers
Authorization: Bearer <token>
Content-Type: application/json

{
  "firstName": "Ahmed",
  "lastName": "Hassan",
  "email": "ahmed@example.com",
  "phone": "+966501234567",
  "address": "Street Name",
  "city": "Riyadh"
}
```

#### Update Customer
```
PUT /customers/{id}
Authorization: Bearer <token>
Content-Type: application/json
```

#### Delete Customer
```
DELETE /customers/{id}
Authorization: Bearer <token>
```

#### Add Loyalty Points
```
POST /customers/{customerId}/loyalty-points
Authorization: Bearer <token>
Content-Type: application/json

100
```

### Suppliers

#### Get All Suppliers
```
GET /suppliers
Authorization: Bearer <token>
```

#### Get Supplier by ID
```
GET /suppliers/{id}
Authorization: Bearer <token>
```

#### Create Supplier
```
POST /suppliers
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "ABC Supplies",
  "contactPerson": "John Doe",
  "email": "john@abcsupplies.com",
  "phone": "+1234567890",
  "address": "123 Business St",
  "city": "New York",
  "country": "USA",
  "creditLimit": 50000
}
```

### Orders

#### Get All Orders
```
GET /orders
Authorization: Bearer <token>
```

#### Get Order by ID
```
GET /orders/{id}
Authorization: Bearer <token>
```

#### Create Order
```
POST /orders
Authorization: Bearer <token>
Content-Type: application/json

{
  "customerId": 1,
  "paymentMethod": "Cash",
  "items": [
    {
      "productId": 1,
      "quantity": 2,
      "unitPrice": 2.50
    }
  ]
}
```

#### Get Orders by Customer
```
GET /orders/customer/{customerId}
Authorization: Bearer <token>
```

### Reports

#### Get Sales Report
```
GET /reports/sales?startDate=2026-01-01&endDate=2026-12-31
Authorization: Bearer <token>
```

#### Get Inventory Report
```
GET /reports/inventory
Authorization: Bearer <token>
```

#### Get Customer Activity Report
```
GET /reports/customer-activity?customerId=1
Authorization: Bearer <token>
```

#### Get Top Products Report
```
GET /reports/top-products?limit=10
Authorization: Bearer <token>
```

## Error Handling

All errors follow this format:
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "errors": ["error code"],
  "timestamp": "2026-09-06T00:00:00Z"
}
```

## Status Codes

- `200 OK` - Success
- `201 Created` - Resource created
- `400 Bad Request` - Invalid input
- `401 Unauthorized` - Missing or invalid token
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

## Pagination

Supported query parameters for list endpoints:
- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 10, max: 100)

## Filtering

Supported query parameters for filtering:
- `status` - Filter by status
- `category` - Filter by category
- `isActive` - Filter active/inactive items

## Sorting

Supported query parameters for sorting:
- `sortBy` - Column to sort by
- `sortOrder` - asc or desc (default: asc)

---

**API Version**: 1.0.0  
**Last Updated**: 2026-09-06
