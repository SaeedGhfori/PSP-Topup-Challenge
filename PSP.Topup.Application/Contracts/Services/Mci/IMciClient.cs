namespace PSP.Topup.Application.Contracts.Services.Mci;

public interface IMciClient
{
    Task<MciTopupResponse> TopupAsync(
        MciTopupRequest request,
        CancellationToken cancellationToken = default);
}
