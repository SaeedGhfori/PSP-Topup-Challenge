
using PSP.Mock.Bank.Api.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBankServices(this IServiceCollection services)
    {
        services.AddSingleton<IBankService, BankService>();

        return services;
    }
}
