using FluentValidation;

namespace PSP.Topup.Api.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                title = "Validation Error",
                status = 400,
                errors = ex.Errors.Select(x => new
                {
                    x.PropertyName,
                    x.ErrorMessage
                }),
                traceId = context.TraceIdentifier
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled Exception");

            context.Response.StatusCode = 500;

            await context.Response.WriteAsJsonAsync(new
            {
                title = "Internal Server Error",
                status = 500,
                traceId = context.TraceIdentifier
            });
        }
    }
}
