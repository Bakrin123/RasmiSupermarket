namespace RasmiSupermarket.Core.Exceptions;

/// <summary>
/// Custom exception for API errors
/// </summary>
public class ApiException : Exception
{
    public int StatusCode { get; set; }
    public string? ErrorCode { get; set; }

    public ApiException(string message, int statusCode = 500, string? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}

public class NotFoundException : ApiException
{
    public NotFoundException(string message, string? errorCode = null)
        : base(message, 404, errorCode)
    {
    }
}

public class BadRequestException : ApiException
{
    public BadRequestException(string message, string? errorCode = null)
        : base(message, 400, errorCode)
    {
    }
}

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message, string? errorCode = null)
        : base(message, 401, errorCode)
    {
    }
}

public class ForbiddenException : ApiException
{
    public ForbiddenException(string message, string? errorCode = null)
        : base(message, 403, errorCode)
    {
    }
}
