using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PSP.Interceptors;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Repositories;
using PSP.Topup.Persistence.Repositories;
using PSP.Topup.Persistence.UnitOfWorks;

namespace PSP.Topup.Persistence.DependencyInjection;

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
