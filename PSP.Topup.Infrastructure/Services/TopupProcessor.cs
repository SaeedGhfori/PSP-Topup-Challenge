using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Messaging.Abstractions;
using PSP.Topup.Application.Abstractions;
using PSP.Topup.Application.Integrations;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Enums;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Infrastructure.Services;

public sealed class TopupProcessor : ITopupProcessor
{
    private readonly ITopupRepository _repository;
    private readonly ITopupProvider _topupProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TopupProcessor> _logger;
    private readonly IMessageBus _messageBus;

    public TopupProcessor(
        ITopupRepository repository,
        ITopupProvider topupProvider,
        IUnitOfWork unitOfWork,
        ILogger<TopupProcessor> logger,
        IMessageBus messageBus)
    {
        _repository = repository;
        _topupProvider = topupProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _messageBus = messageBus;
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
            await _topupProvider.TopupAsync(
                new TopupRequest(
                    transaction.PhoneNumber.Value,
                    transaction.Amount.Value,
                    transaction.Id.ToString()),
                cancellationToken);

        if (response.Success)
        {
            transaction.MarkSucceeded(
                response.ReferenceNumber ?? string.Empty);
        }
        else
        {
            transaction.MarkFailed(
                response.Message);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        await _messageBus.PublishAsync(
    new TopupCompletedIntegrationEvent(
        transaction.Id,
        transaction.Status == TransactionStatus.TopupSucceeded,
        transaction.ProviderReference,
        transaction.FailureReason),
    cancellationToken);

        _logger.LogInformation(
            "Topup processed {TransactionId}",
            transactionId);
    }
}
