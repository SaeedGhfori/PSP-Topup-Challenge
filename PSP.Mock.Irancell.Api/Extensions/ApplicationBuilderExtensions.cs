using PSP.Mock.Irancell.Api.Middlewares;

namespace PSP.Mock.Irancell.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
