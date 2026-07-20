namespace PSP.Topup.Application.Contracts.Mci;

public interface IMciClient
{
    Task<MciTopupResponse> TopupAsync(
        MciTopupRequest request,
        CancellationToken cancellationToken = default);
}
