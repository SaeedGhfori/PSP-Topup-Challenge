using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using PSP.Topup.Application.Contracts.Mci;
using PSP.Topup.Application.Contracts.Services;
using PSP.Topup.Infrastructure.Clients;
using PSP.Topup.Infrastructure.Configuration;
using PSP.Topup.Infrastructure.Services;

namespace PSP.Topup.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MciOptions>(
            configuration.GetSection(MciOptions.SectionName));

        services.AddHttpClient<IMciClient, MciClient>(
            (provider, client) =>
            {
                var options =
                    provider.GetRequiredService<IOptions<MciOptions>>().Value;

                client.BaseAddress =
                    new Uri(options.BaseUrl);

                client.Timeout =
                    TimeSpan.FromSeconds(options.Timeout);
            });

        services.AddScoped<ITopupProcessor, TopupProcessor>();

        return services;
    }
}
