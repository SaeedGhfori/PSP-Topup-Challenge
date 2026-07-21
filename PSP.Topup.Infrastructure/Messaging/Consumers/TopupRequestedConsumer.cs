using MassTransit;

using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Topup.Application.Abstractions;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Entities;
using PSP.Topup.Domain.Enums;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Infrastructure.Messaging.Consumers;

public sealed class TopupRequestedConsumer
    : IConsumer<TopupRequestedIntegrationEvent>
{
    private readonly ITopupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITopupProcessor _processor;
    private readonly ILogger<TopupRequestedConsumer> _logger;

    public TopupRequestedConsumer(
        ITopupRepository repository,
        IUnitOfWork unitOfWork,
        ITopupProcessor processor,
        ILogger<TopupRequestedConsumer> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _processor = processor;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<TopupRequestedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received TopupRequestedIntegrationEvent {TransactionId}",
            message.TransactionId);

        var exists =
            await _repository.GetByIdempotencyKeyAsync(
                message.IdempotencyKey,
                context.CancellationToken);

        if (exists is not null)
        {
            _logger.LogInformation(
                "Duplicate Topup ignored.");

            return;
        }

        var transaction = TopupTransaction.Create(
            message.TransactionId,
            PhoneNumber.Create(message.PhoneNumber),
            Money.Create(message.Amount),
            (MobileOperator)message.OperatorId,
            message.IdempotencyKey);

        await _repository.AddAsync(
            transaction,
            context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(
            context.CancellationToken);

        await _processor.ProcessAsync(
            transaction.Id,
            context.CancellationToken);
    }
}
