using Microsoft.Extensions.Logging;

using PSP.Topup.Application.Contracts.Services;
using PSP.Topup.Application.Contracts.Services.Mci;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Infrastructure.Services;

public sealed class TopupProcessor : ITopupProcessor
{
    private readonly ITopupRepository _repository;
    private readonly IMciClient _mciClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TopupProcessor> _logger;

    public TopupProcessor(
        ITopupRepository repository,
        IMciClient mciClient,
        IUnitOfWork unitOfWork,
        ILogger<TopupProcessor> logger)
    {
        _repository = repository;
        _mciClient = mciClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ProcessAsync(
        Guid transactionId,
        CancellationToken cancellationToken)
    {
        var transaction =
            await _repository.GetByIdAsync(
                transactionId,
                cancellationToken);

        if (transaction is null)
            throw new Exception("Transaction not found.");

        var response =
            await _mciClient.TopupAsync(
                new MciTopupRequest(
                    transaction.PhoneNumber.Value,
                    transaction.Amount.Value,
                    transaction.Id.ToString()),
                cancellationToken);

        if (response.Success)
        {
            transaction.MarkSucceeded(
                response.ReferenceNumber);
        }
        else
        {
            transaction.MarkFailed(
                response.Message);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        _logger.LogInformation(
            "Topup processed {TransactionId}",
            transactionId);
    }
}
