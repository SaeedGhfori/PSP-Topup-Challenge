using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Payment.Domain.Common;
using PSP.Payment.Persistence.Context;
using PSP.Payment.Persistence.UnitOfWorks;

namespace PSP.Payment.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddSingleton<AuditInterceptor>();

        services.AddDbContext<PaymentDbContext>((provider, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName);
                });

            //options.AddInterceptors(provider.GetRequiredService<AuditInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
