using Microsoft.AspNetCore.Mvc;

using PSP.Mock.Irancell.Api.Contracts;
using PSP.Mock.Irancell.Api.Services;

namespace PSP.Mock.Irancell.Api.Controllers;

[ApiController]
[Route("api/v1/irancell")]
public sealed class IrancellController : ControllerBase
{
    private readonly IIrancellService _service;

    public IrancellController(IIrancellService service)
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
