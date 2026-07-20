using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PSP.Payment.Application.Contracts.Bank;
using PSP.Payment.Infrastructure.Clients;

namespace PSP.Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IBankClient, BankClient>(client =>
        {
            client.BaseAddress =
                new Uri(configuration["Bank:BaseUrl"]!);
        });

        return services;
    }
}
