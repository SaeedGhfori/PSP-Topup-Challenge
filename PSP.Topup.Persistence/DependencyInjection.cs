using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Topup.Persistence.Context;

namespace PSP.Topup.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TopupDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Postgres"));
        });

        return services;
    }
}
