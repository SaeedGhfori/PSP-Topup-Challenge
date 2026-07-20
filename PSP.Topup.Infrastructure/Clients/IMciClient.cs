using PSP.Topup.Infrastructure.Contracts.Requests;
using PSP.Topup.Infrastructure.Contracts.Responses;

namespace PSP.Topup.Infrastructure.Clients;

public interface IMciClient
{
    Task<MciTopupResponse> TopupAsync(
        MciTopupRequest request,
        CancellationToken cancellationToken = default);
}
