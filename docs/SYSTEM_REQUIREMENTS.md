# System Requirements

## Development Environment

### Minimum Requirements

- **OS**: Windows 10/11, macOS 10.15+, Ubuntu 18.04+
- **RAM**: 4 GB minimum (8 GB recommended)
- **Storage**: 5 GB free space
- **CPU**: 2.0 GHz dual-core processor

### Required Software

#### .NET Runtime
- **.NET 6.0 SDK** or higher ([Download](https://dotnet.microsoft.com/download/dotnet/6.0))
  - Include ASP.NET Core runtime
  - Includes Entity Framework Core tools

#### Database
- **SQL Server 2019** or higher ([Download](https://www.microsoft.com/en-us/sql-server/sql-server-2019))
  - Express edition is sufficient for development
  - Developer edition for advanced features
  - Or use Docker container (recommended)

#### IDE/Editor

**Option 1: Visual Studio 2022**
- Community Edition (free)
- Professional Edition
- Enterprise Edition
- Workload: ASP.NET and web development

**Option 2: Visual Studio Code**
- C# extension (Microsoft)
- REST Client extension (for API testing)
- SQL Server extension (optional)

**Option 3: JetBrains Rider**
- Full-featured .NET IDE
- Free for open-source projects

#### Git
- **Git 2.30+** ([Download](https://git-scm.com/download))
- GitHub CLI (optional) for enhanced GitHub integration

#### Package Manager
- **NuGet** (included with .NET SDK)

### Development Dependencies

```xml
<!-- From RasmiSupermarket.API.csproj -->
Microsoft.EntityFrameworkCore.SqlServer 6.0.0
Swashbuckle.AspNetCore 6.2.3
AutoMapper.Extensions.Microsoft.DependencyInjection 11.0.0
FluentValidation.DependencyInjectionExtensions 11.0.0
System.IdentityModel.Tokens.Jwt 6.25.1
Microsoft.IdentityModel.Tokens 6.25.1
```

## Production Environment

### Server Requirements

- **OS**: Windows Server 2016+ or Linux (Ubuntu 18.04+, CentOS 7+)
- **RAM**: 8 GB minimum (16 GB recommended)
- **Storage**: 50 GB for data and logs
- **CPU**: 4-core processor (8-core recommended)
- **Network**: 100 Mbps minimum (1 Gbps recommended)

### Database Server

- **SQL Server 2019** or higher
  - Standard Edition or higher
  - Enterprise for high availability
  - Always On availability groups recommended
  - Backup strategy essential

**Specifications:**
- RAM: 16 GB minimum
- Storage: 100 GB SSD minimum
- Backup storage: Equal to database size

### Web Server

- **IIS 8.5+** (Windows)
  - Application Pool settings configured
  - URL Rewrite module installed
  - SSL/TLS certificate installed

OR

- **Nginx/Apache** (Linux)
  - Configured as reverse proxy
  - SSL/TLS termination
  - Load balancing setup

### Docker Deployment

**Docker Desktop/Server**
- Docker 20.10+
- Docker Compose 1.29+
- 4+ GB RAM allocation to Docker

## Network Requirements

### Ports

| Service | Port | Protocol | Purpose |
|---------|------|----------|----------|
| API | 5000/5001 | HTTP/HTTPS | Web API |
| SQL Server | 1433 | TCP | Database |
| SSH | 22 | TCP | Server access |
| HTTPS | 443 | HTTPS | Secure web |
| HTTP | 80 | HTTP | Web traffic |

### Firewall Rules

- Allow inbound 5000/5001 for API
- Allow inbound 443 for HTTPS
- Allow inbound 1433 for database (internal only)
- Allow outbound SMTP for email notifications

## Security Requirements

### Certificates

- SSL/TLS certificate for HTTPS
  - Self-signed for development
  - CA-signed for production
  - Let's Encrypt recommended

### Credentials

- Database passwords: 12+ characters, mixed case, numbers, symbols
- JWT secret key: 32+ characters
- API keys: Generated and stored securely

### Compliance

- HTTPS enforced in production
- Database encryption at rest
- Audit logging enabled
- Regular security updates

## Performance Considerations

### Caching

- Redis 6.0+ (optional, for distributed caching)
- In-memory cache for development
- Database query caching

### Monitoring

- Application Insights (optional)
- ELK Stack (optional)
- Basic logging to files

### Scaling

- Load balancer for multiple API instances
- Connection pooling for database
- Read replicas for reporting

## Browser Support

### Swagger UI
- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

### Frontend (Future)
- Modern browsers with ES6 support
- Mobile browsers (iOS Safari, Chrome Mobile)

## Development Workflow

### Local Development

```bash
# Check prerequisites
dotnet --version  # Should be 6.0+
sqlcmd -? # Should work
git --version # Should be 2.30+

# Clone repository
git clone https://github.com/Bakrin123/RasmiSupermarket.git

# Restore packages
cd RasmiSupermarket
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Start application
dotnet run --project src/RasmiSupermarket.API
```

### Docker Development

```bash
# Build Docker image
docker build -t rasmi-supermarket:latest .

# Start with Docker Compose
docker-compose up -d

# Check logs
docker-compose logs -f api
```

## Troubleshooting

### .NET SDK Issues

```bash
# List installed SDKs
dotnet --list-sdks

# Install specific version
# Download from https://dotnet.microsoft.com/download/dotnet/6.0
```

### SQL Server Connection Issues

```bash
# Test connection
sqlcmd -S localhost -U sa -P <password>

# On Linux
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P <password>
```

### Port Already in Use

```bash
# Windows
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Linux/macOS
lsof -i :5000
kill -9 <PID>
```

## Environment Variables

### Development

```bash
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:5001;http://localhost:5000
ConnectionStrings__DefaultConnection=Server=localhost;Database=RasmiSupermarket;User Id=sa;Password=YourPassword;
```

### Production

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://0.0.0.0:5001
ConnectionStrings__DefaultConnection=Server=<server>;Database=RasmiSupermarket;User Id=<user>;Password=<password>;
AppSettings__JwtSecretKey=<secure-key-here>
```

## Deployment Checklist

- [ ] .NET 6.0 SDK installed
- [ ] SQL Server running and accessible
- [ ] Database created and migrations applied
- [ ] SSL/TLS certificate installed
- [ ] Firewall rules configured
- [ ] Environment variables set
- [ ] Backups configured
- [ ] Monitoring setup
- [ ] Logging configured
- [ ] Health checks working

---

**System Requirements Version**: 1.0.0  
**Last Updated**: 2026-09-06
