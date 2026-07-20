using Polly;
using Polly.Extensions.Http;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                3,
                retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));
    }

    public static IAsyncPolicy<HttpResponseMessage> CircuitBreaker()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                5,
                TimeSpan.FromSeconds(30));
    }
}
