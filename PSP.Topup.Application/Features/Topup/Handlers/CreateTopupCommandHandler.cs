using MediatR;

using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Messaging.Abstractions;
using PSP.Topup.Application.Features.Topup.Commands;
using PSP.Topup.Application.Features.Topup.DTOs;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Entities;
using PSP.Topup.Domain.Enums;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Application.Features.Topup.Handlers;

public sealed class CreateTopupCommandHandler
    : IRequestHandler<CreateTopupCommand, CreateTopupResponse>
{
    private readonly ITopupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<CreateTopupCommandHandler> _logger;

    public CreateTopupCommandHandler(
        ITopupRepository repository,
        IUnitOfWork unitOfWork,
        IMessageBus messageBus,
        ILogger<CreateTopupCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task<CreateTopupResponse> Handle(
        CreateTopupCommand request,
        CancellationToken cancellationToken)
    {
        var duplicate =
            await _repository.GetByIdempotencyKeyAsync(
                request.IdempotencyKey,
                cancellationToken);

        if (duplicate is not null)
        {
            return new CreateTopupResponse(
                duplicate.Id,
                duplicate.Status.ToString());
        }

        var transaction = TopupTransaction.Create(
            Guid.Empty,
            PhoneNumber.Create(request.PhoneNumber),
            Money.Create(request.Amount),
            (MobileOperator)request.OperatorId,
            request.IdempotencyKey);

        await _repository.AddAsync(
            transaction,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        await _messageBus.PublishAsync(
            new TopupRequestedIntegrationEvent(
                transaction.Id,
                request.PhoneNumber,
                request.Amount,
                request.OperatorId,
                request.IdempotencyKey),
            cancellationToken);

        _logger.LogInformation(
            "Topup Request Published. TransactionId:{Id}",
            transaction.Id);

        return new CreateTopupResponse(
            transaction.Id,
            transaction.Status.ToString());
    }
}
