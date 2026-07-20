using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using PSP.Topup.Infrastructure.Contracts.Requests;
using PSP.Topup.Infrastructure.Contracts.Responses;

namespace PSP.Topup.Infrastructure.Clients;

public sealed class MciClient : IMciClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MciClient> _logger;

    public MciClient(
        HttpClient httpClient,
        ILogger<MciClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<MciTopupResponse> TopupAsync(
        MciTopupRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Sending topup request to MCI. RequestId={RequestId}",
            request.RequestId);

        using var response =
            await _httpClient.PostAsJsonAsync(
                "topup",
                request,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<MciTopupResponse>(
                cancellationToken: cancellationToken);

        if (result is null)
            throw new InvalidOperationException("MCI returned empty response.");

        _logger.LogInformation(
            "MCI Response Status={Status}",
            result.Status);

        return result;
    }
}
