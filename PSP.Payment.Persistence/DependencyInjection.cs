using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Payment.Domain.Common;
using PSP.Payment.Domain.Repositories;
using PSP.Payment.Persistence.Context;
using PSP.Payment.Persistence.Repositories;

namespace PSP.Payment.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PaymentDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
