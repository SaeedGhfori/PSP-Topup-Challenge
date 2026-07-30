using PSP.Mock.Irancell.Api.Contracts;
using PSP.Mock.Irancell.Api.Enum;
using PSP.Mock.Irancell.Api.Helpers;

namespace PSP.Mock.Irancell.Api.Services;

public sealed class IrancellService : IIrancellService
{
    public async Task<TopupResponse> TopupAsync(TopupRequest request)
    {
        await Task.Delay(Random.Shared.Next(300, 800));

        var lastDigit = request.MobileNumber[^1];

        return lastDigit switch
        {
            '1' => new TopupResponse(
                true,
                TopupStatus.Success.ToString(),
                "Topup completed successfully.",
                ReferenceGenerator.Generate()),

            '2' => new TopupResponse(
                true,
                TopupStatus.Pending.ToString(),
                "Request accepted and is being processed.",
                ReferenceGenerator.Generate()),

            '3' => new TopupResponse(
                false,
                TopupStatus.Failed.ToString(),
                "Topup failed.",
                null),

            '4' => new TopupResponse(
                false,
                TopupStatus.InvalidMobile.ToString(),
                "Invalid mobile number.",
                null),

            '5' => new TopupResponse(
                false,
                TopupStatus.Duplicate.ToString(),
                "Duplicate request.",
                null),

            '6' => throw new TimeoutException("Irancell timeout."),

            '7' => new TopupResponse(
                false,
                TopupStatus.SystemError.ToString(),
                "Internal Irancell error.",
                null),

            _ => new TopupResponse(
                true,
                TopupStatus.Success.ToString(),
                "Topup completed successfully.",
                ReferenceGenerator.Generate())
        };
    }

    public async Task<InquiryResponse> InquiryAsync(string referenceNumber)
    {
        await Task.Delay(200);

        return new InquiryResponse(
            referenceNumber,
            TopupStatus.Success.ToString(),
            "Topup completed successfully.");
    }
}
