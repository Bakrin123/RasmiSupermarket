# Contributing Guide

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/yourusername/RasmiSupermarket.git`
3. Create a feature branch: `git checkout -b feature/your-feature-name`
4. Make your changes
5. Commit with clear messages: `git commit -am 'Add your feature'`
6. Push to your fork: `git push origin feature/your-feature-name`
7. Submit a Pull Request

## Code Style

### C# Conventions
- Use PascalCase for class names and public methods
- Use camelCase for private fields and parameters
- Use meaningful, descriptive names
- Maximum line length: 120 characters

### Example
```csharp
public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;

    public async Task<ProductDto> GetProductAsync(int id)
    {
        // Implementation
    }
}
```

## Commit Message Format

```
<type>: <subject>

<body>

<footer>
```

### Types
- `feat`: A new feature
- `fix`: A bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, missing semicolons, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Build process, dependencies, etc.

### Example
```
feat: Add customer loyalty points feature

Implement functionality to add and track loyalty points for customers.
- Add loyalty points to customer profile
- Update customer endpoints to include loyalty data
- Add validation for points amounts

Closes #123
```

## Pull Request Process

1. Update documentation for any new features
2. Add tests for new functionality
3. Ensure all tests pass: `dotnet test`
4. Update CHANGELOG.md
5. Request review from maintainers

## Testing

### Unit Tests
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run specific test
dotnet test --filter ProductServiceTests
```

### Test File Naming
- `{ClassName}Tests.cs` for unit tests
- `{ClassName}IntegrationTests.cs` for integration tests

### Test Structure
```csharp
public class ProductServiceTests
{
    [Fact]
    public async Task GetProduct_WithValidId_ReturnsProduct()
    {
        // Arrange
        var productService = new ProductService(_unitOfWork, _mapper, _logger);
        
        // Act
        var result = await productService.GetProductByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }
}
```

## Documentation

### Code Comments
- Add XML documentation comments to public methods
- Explain complex logic with inline comments

### Example
```csharp
/// <summary>
/// Gets a product by its ID.
/// </summary>
/// <param name="id">The product ID</param>
/// <returns>API response with product data</returns>
public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
{
    // Implementation
}
```

### Documentation Files
- Update README.md for major changes
- Update docs/API_DOCUMENTATION.md for API changes
- Update docs/ARCHITECTURE.md for structural changes

## Issue Reporting

### Bug Reports
Include:
- Clear description of the bug
- Steps to reproduce
- Expected behavior
- Actual behavior
- Screenshots if applicable

### Feature Requests
Include:
- Clear description of the feature
- Use case and benefits
- Proposed implementation (if any)
- Examples or mockups

## Code Review Process

1. Assign reviewers
2. Address feedback and comments
3. Push updates to the same branch
4. Re-request review after changes
5. Merge after approval

## Release Process

1. Update version numbers following [Semantic Versioning](https://semver.org/)
2. Update CHANGELOG.md
3. Create release tag: `git tag v1.0.0`
4. Push tag: `git push origin v1.0.0`
5. Create release notes on GitHub

## Development Workflow

1. **Create branch from main**: `git checkout -b feature/xyz`
2. **Make changes and test locally**
3. **Create Pull Request**: Describe changes clearly
4. **Code review**: Address feedback
5. **Merge to main**: Squash commits if necessary
6. **Delete feature branch**: Clean up

## Development Environment Setup

```bash
# Install dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Start development server
dotnet run --project src/RasmiSupermarket.API
```

## Troubleshooting

### Common Issues

**Issue**: Migration conflicts
**Solution**: 
```bash
dotnet ef migrations remove
dotnet ef migrations add YourMigration
```

**Issue**: Package conflicts
**Solution**:
```bash
dotnet clean
dotnet restore
dotnet build
```

## Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [RESTful API Design](https://restfulapi.net/)

## Contact

For questions or discussions:
- Create an issue on GitHub
- Join our community discussions

---

**Version**: 1.0.0  
**Last Updated**: 2026-09-06
