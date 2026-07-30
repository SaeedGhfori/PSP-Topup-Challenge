using System.Net;
using System.Text.Json;

namespace PSP.Mock.Irancell.Api.Middlewares;

public sealed class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (TimeoutException ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.GatewayTimeout,
                ex.Message);
        }
        catch (Exception ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    private static async Task WriteResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            Success = false,
            StatusCode = context.Response.StatusCode,
            Message = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
