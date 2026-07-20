using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Repositories;
using PSP.Topup.Persistence.Context;
using PSP.Topup.Persistence.Interceptors;
using PSP.Topup.Persistence.Repositories;


namespace PSP.Topup.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<AuditInterceptor>();

        services.AddDbContext<TopupDbContext>((provider, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));

            options.AddInterceptors(
                provider.GetRequiredService<AuditInterceptor>());
        });

        services.AddScoped<ITopupRepository, TopupRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
