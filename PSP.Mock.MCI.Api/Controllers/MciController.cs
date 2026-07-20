using Microsoft.AspNetCore.Mvc;

using PSP.Mock.MCI.Api.Contracts;
using PSP.Mock.MCI.Api.Services;

namespace PSP.Mock.MCI.Api.Controllers;

[ApiController]
[Route("api/v1/mci")]
public sealed class MciController : ControllerBase
{
    private readonly IMciService _service;

    public MciController(IMciService service)
    {
        _service = service;
    }

    [HttpPost("topup")]
    public async Task<IActionResult> Topup(
        TopupRequest request)
    {
        var response =
            await _service.TopupAsync(request);

        return Ok(response);
    }

    [HttpGet("topup/{referenceNumber}")]
    public async Task<IActionResult> Inquiry(
        string referenceNumber)
    {
        var response =
            await _service.InquiryAsync(referenceNumber);

        return Ok(response);
    }
}
