using Microsoft.Extensions.Logging;

public sealed class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(
        ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Calling Topup Provider {Method} {Url}",
            request.Method,
            request.RequestUri);

        var response =
            await base.SendAsync(request, cancellationToken);

        _logger.LogInformation(
            "Topup Provider Response {StatusCode}",
            response.StatusCode);

        return response;
    }
}
