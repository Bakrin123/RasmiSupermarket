using RasmiSupermarket.Core.Exceptions;
using RasmiSupermarket.Core.Responses;
using System.Text.Json;

namespace RasmiSupermarket.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ApiResponse<object>();

        if (exception is ApiException apiException)
        {
            context.Response.StatusCode = apiException.StatusCode;
            response = new ApiResponse<object>
            {
                Success = false,
                Message = apiException.Message,
                Errors = new List<string> { apiException.ErrorCode ?? "Error" }
            };
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            response = new ApiResponse<object>
            {
                Success = false,
                Message = "An unexpected error occurred",
                Errors = new List<string> { exception.Message }
            };
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
