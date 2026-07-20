using Microsoft.AspNetCore.Mvc;

using PSP.Mock.Bank.Api.Contracts;
using PSP.Mock.Bank.Api.Services;

namespace PSP.Mock.Bank.Api.Controllers;

[ApiController]
[Route("api/v1/bank")]
public sealed class BankController : ControllerBase
{
    private readonly IBankService _service;

    public BankController(IBankService service)
    {
        _service = service;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase(
        PurchaseRequest request)
    {
        var response =
            await _service.PurchaseAsync(request);

        return Ok(response);
    }

    [HttpPost("confirmation")]
    public async Task<IActionResult> Confirmation(
        ConfirmationRequest request)
    {
        var response =
            await _service.ConfirmationAsync(request);

        return Ok(response);
    }

    [HttpPost("reversal")]
    public async Task<IActionResult> Reversal(
        ReversalRequest request)
    {
        var response =
            await _service.ReversalAsync(request);

        return Ok(response);
    }

    [HttpPost("balance")]
    public async Task<IActionResult> Balance(
        BalanceRequest request)
    {
        var response =
            await _service.BalanceAsync(request);

        return Ok(response);
    }
}
