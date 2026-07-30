using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using PSP.Topup.Application.Integrations;

namespace PSP.Topup.Infrastructure.Clients;

public sealed class IrancellTopupProvider : ITopupProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IrancellTopupProvider> _logger;

    public IrancellTopupProvider(
        HttpClient httpClient,
        ILogger<IrancellTopupProvider> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<TopupResponse> TopupAsync(
        TopupRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Sending topup request to Irancell. RequestId={RequestId}",
            request.RequestId);

        using var response =
            await _httpClient.PostAsJsonAsync(
                "topup",
                request,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<TopupResponse>(
                cancellationToken: cancellationToken);

        if (result is null)
            throw new InvalidOperationException("Irancell returned empty response.");

        _logger.LogInformation(
            "Irancell Response Status={Status}",
            result.Status);

        return result;
    }
}
