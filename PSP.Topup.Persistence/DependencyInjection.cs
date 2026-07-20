using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Context;
using PSP.Interceptors;
using PSP.Topup.Persistence.Context;

namespace PSP;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TopupDbContext>((sp, options) =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Postgres"));

            options.AddInterceptors(
                sp.GetRequiredService<AuditInterceptor>(),
                sp.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        services.AddSingleton<AuditInterceptor>();

        services.AddScoped<PublishDomainEventsInterceptor>();



        return services;
    }
}
