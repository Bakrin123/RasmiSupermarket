# Architecture Guide - RasmiSupermarket

## Project Structure

```
RasmiSupermarket/
├── src/
│   ├── RasmiSupermarket.API/
│   │   ├── Controllers/           # API endpoints
│   │   ├── Middleware/            # Custom middleware
│   │   ├── Authentication/        # Auth handlers
│   │   ├── Program.cs             # Startup configuration
│   │   └── appsettings.json       # Configuration
│   │
│   ├── RasmiSupermarket.Core/
│   │   ├── Entities/              # Domain models
│   │   ├─�� DTOs/                  # Data transfer objects
│   │   ├── Services/              # Business logic
│   │   ├── Validations/           # Fluent validations
│   │   ├── Mappings/              # AutoMapper profiles
│   │   ├── Exceptions/            # Custom exceptions
│   │   └── Responses/             # API response wrappers
│   │
│   └── RasmiSupermarket.Data/
│       ├── RasmiSupermarketContext.cs   # DbContext
│       └── Repositories/          # Data access layer
│
├── tests/
│   └── RasmiSupermarket.Tests/    # Unit and integration tests
│
├── docs/                          # Documentation
├── scripts/                       # Database scripts
└── README.md
```

## Architecture Layers

### 1. Presentation Layer (API)
- **Responsibility**: Handle HTTP requests/responses
- **Components**: Controllers, Middleware
- **Technologies**: ASP.NET Core, Swagger

### 2. Business Logic Layer (Core)
- **Responsibility**: Implement business rules
- **Components**: Services, Entities, DTOs, Validations
- **Technologies**: AutoMapper, FluentValidation

### 3. Data Access Layer (Data)
- **Responsibility**: Interact with database
- **Components**: Repository Pattern, Unit of Work, DbContext
- **Technologies**: Entity Framework Core, SQL Server

## Design Patterns Used

### Repository Pattern
```
IRepository<T> → Repository<T> → DbContext
```
Provides abstraction for data access operations.

### Unit of Work Pattern
```
IUnitOfWork → Manages multiple repositories
```
Ensures atomic transactions across multiple repositories.

### Dependency Injection
```
Registered in Program.cs
- Controllers receive dependencies through constructors
```

### AutoMapper
```
Entity → DTO mapping
Reduces boilerplate code
```

### Service Layer Pattern
```
Controller → Service → Repository → Database
```
Separates concerns and improves testability.

## Key Concepts

### Entity Models
- Represent database tables
- Located in `Core/Entities/`
- Examples: Product, Customer, Order, User

### DTOs (Data Transfer Objects)
- Transfer data between layers
- Located in `Core/DTOs/`
- Separate from entities for flexibility

### Services
- Implement business logic
- Located in `Core/Services/`
- Injected into controllers

### Controllers
- Handle HTTP requests
- Located in `API/Controllers/`
- Return standardized API responses

### Middleware
- Process requests/responses
- Located in `API/Middleware/`
- Example: Exception handling

## Database Configuration

### DbContext
```csharp
public class RasmiSupermarketContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    // ... other DbSets
}
```

### Migrations
```bash
# Create migration
dotnet ef migrations add MigrationName

# Apply migration
dotnet ef database update
```

## Security

### Authentication
- JWT token-based
- Implemented in `AuthService`
- Token generation on successful login

### Authorization
- Role-based access control (RBAC)
- Roles: Admin, Manager, User, Cashier
- Implemented in controllers with `[Authorize]` attributes

### Password Security
- SHA256 hashing
- Salt-based approach (can be enhanced)
- Implemented in `AuthService.HashPassword()`

## API Response Format

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; }
    public DateTime Timestamp { get; set; }
}
```

## Exception Handling

### Custom Exceptions
- `ApiException`: Base exception
- `NotFoundException`: 404
- `BadRequestException`: 400
- `UnauthorizedException`: 401
- `ForbiddenException`: 403

### Middleware Exception Handler
- Centralized error handling
- Returns standardized error responses
- Logs all exceptions

## Validation Strategy

### FluentValidation
- Separate validator classes
- Located in `Core/Validations/`
- Applied in controllers or service layer

### Example
```csharp
public class CreateProductDtoValidator : AbstractValidator<ProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(3, 255);
    }
}
```

## Logging

### Implementation
- Built-in ASP.NET Core logging
- Injected into services and controllers
- Configured in `appsettings.json`

### Log Levels
- Information: General application flow
- Warning: Unexpected situations
- Error: Exceptions and failures

## Dependency Injection (DI) Configuration

```csharp
// In Program.cs
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
```

## CORS Configuration

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RasmiSupermarket;..."
  },
  "AppSettings": {
    "JwtSecretKey": "your-secret-key",
    "JwtExpireMinutes": 1440
  }
}
```

## Best Practices

1. **Separation of Concerns**: Each layer has specific responsibilities
2. **DRY (Don't Repeat Yourself)**: Reusable services and repositories
3. **SOLID Principles**: Dependency Injection, Interface Segregation
4. **Error Handling**: Centralized exception handling
5. **Logging**: Comprehensive logging throughout the application
6. **Validation**: Input validation before processing
7. **Security**: JWT authentication, password hashing
8. **Testing**: Mockable dependencies for unit testing

## Testing Strategy

### Unit Tests
- Test individual services
- Mock dependencies using Moq
- Located in `tests/RasmiSupermarket.Tests/`

### Integration Tests
- Test API endpoints
- Use test database
- Verify data persistence

---

**Architecture Version**: 1.0.0  
**Last Updated**: 2026-09-06
