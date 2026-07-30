namespace PSP.Topup.Application.Integrations;

public interface ITopupProvider
{
    Task<TopupResponse> TopupAsync(
        TopupRequest request,
        CancellationToken cancellationToken = default);
}
