using PSP.Mock.Bank.Api.Middlewares;

namespace PSP.Mock.Bank.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalException(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
