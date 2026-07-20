using MediatR;

using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Messaging.Abstractions;
using PSP.Payment.Application.Contracts.Bank;
using PSP.Payment.Application.Features.DTOs;
using PSP.Payment.Application.Features.Payments.Commands;
using PSP.Payment.Domain.Common;
using PSP.Payment.Domain.Entities;
using PSP.Payment.Domain.Repositories;
using PSP.Payment.Domain.ValueObjects;

namespace PSP.Payment.Application.Features.Payments.Handlers;

public sealed class CreatePurchaseCommandHandler
    : IRequestHandler<CreatePurchaseCommand, CreatePurchaseResponse>
{
    private readonly IPaymentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBankClient _bankClient;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<CreatePurchaseCommandHandler> _logger;

    public CreatePurchaseCommandHandler(
        IPaymentRepository repository,
        IUnitOfWork unitOfWork,
        IBankClient bankClient,
        IMessageBus messageBus,
        ILogger<CreatePurchaseCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _bankClient = bankClient;
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task<CreatePurchaseResponse> Handle(
        CreatePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var duplicate =
            await _repository.GetByIdempotencyKeyAsync(
                request.IdempotencyKey,
                cancellationToken);

        if (duplicate is not null)
        {
            return new CreatePurchaseResponse(
                duplicate.Id,
                duplicate.Status.ToString());
        }

        var transaction = PaymentTransaction.Create(
            Pan.Create(request.Pan),
            Money.Create(request.Amount),
            request.PhoneNumber,
            request.OperatorId,
            TraceNumber.Create(request.TraceNumber),
            TerminalId.Create(request.TerminalId),
            request.IdempotencyKey);

        await _repository.AddAsync(
            transaction,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var purchase =
            await _bankClient.PurchaseAsync(
                new BankPurchaseRequest(
                    request.Pan,
                    request.Amount,
                    request.TerminalId,
                    request.TraceNumber),
                cancellationToken);

        if (!purchase.Success)
        {
            transaction.Fail();

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreatePurchaseResponse(
                transaction.Id,
                transaction.Status.ToString());
        }

        transaction.PurchaseSucceeded(
            purchase.Rrn);

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
            "Purchase completed. Transaction:{Id}",
            transaction.Id);

        return new CreatePurchaseResponse(
            transaction.Id,
            transaction.Status.ToString());
    }
}
