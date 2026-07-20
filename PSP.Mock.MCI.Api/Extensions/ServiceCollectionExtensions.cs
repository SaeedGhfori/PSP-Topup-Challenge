using PSP.Mock.MCI.Api.Services;

namespace PSP.Mock.MCI.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMciServices(this IServiceCollection services)
    {
        services.AddSingleton<IMciService, MciService>();

        return services;
    }
}
