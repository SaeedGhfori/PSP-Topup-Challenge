namespace PSP.Topup.Application.Integrations.Mci;

public interface IMciClient
{
    Task<MciTopupResponse> TopupAsync(
        MciTopupRequest request,
        CancellationToken cancellationToken = default);
}
