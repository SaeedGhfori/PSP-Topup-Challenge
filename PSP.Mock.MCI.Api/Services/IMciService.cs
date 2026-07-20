using PSP.Mock.MCI.Api.Contracts;

namespace PSP.Mock.MCI.Api.Services;

public interface IMciService
{
    Task<TopupResponse> TopupAsync(TopupRequest request);

    Task<InquiryResponse> InquiryAsync(string referenceNumber);
}
