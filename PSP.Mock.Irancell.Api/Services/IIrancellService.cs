using PSP.Mock.Irancell.Api.Contracts;

namespace PSP.Mock.Irancell.Api.Services;

public interface IIrancellService
{
    Task<TopupResponse> TopupAsync(TopupRequest request);

    Task<InquiryResponse> InquiryAsync(string referenceNumber);
}
