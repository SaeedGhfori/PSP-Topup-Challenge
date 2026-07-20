using System.Net.Http;

public sealed class CorrelationIdHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add(
            "X-Correlation-Id",
            Guid.NewGuid().ToString());

        return base.SendAsync(
            request,
            cancellationToken);
    }
}
