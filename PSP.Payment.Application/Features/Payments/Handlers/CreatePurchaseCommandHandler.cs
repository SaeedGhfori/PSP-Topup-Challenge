using MediatR;

using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Messaging.Abstractions;
using PSP.Payment.Application.Contracts.Bank;
using PSP.Payment.Application.Features.Payments.Commands;
using PSP.Payment.Application.Features.Payments.DTOs;
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
        _logger.LogInformation(
            "Starting purchase. TraceNumber:{TraceNumber}",
            request.TraceNumber);

        var duplicate = await GetDuplicateTransactionAsync(
            request,
            cancellationToken);

        if (duplicate is not null)
        {
            _logger.LogInformation(
                "Duplicate purchase request. Transaction:{TransactionId}",
                duplicate.Id);

            return CreateResponse(duplicate);
        }

        var transaction = CreateTransaction(request);

        await SaveTransactionAsync(
            transaction,
            cancellationToken);

        var purchase = await PurchaseFromBankAsync(
            request,
            cancellationToken);

        if (!purchase.Success)
        {
            return await HandleFailedPurchaseAsync(
                transaction,
                cancellationToken);
        }

        return await HandleSuccessfulPurchaseAsync(
            transaction,
            purchase.Rrn,
            request,
            cancellationToken);
    }

    private async Task<PaymentTransaction?> GetDuplicateTransactionAsync(
        CreatePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdempotencyKeyAsync(
            request.IdempotencyKey,
            cancellationToken);
    }

    private static PaymentTransaction CreateTransaction(
        CreatePurchaseCommand request)
    {
        return PaymentTransaction.Create(
            Pan.Create(request.Pan),
            Money.Create(request.Amount),
            request.PhoneNumber,
            request.OperatorId,
            TraceNumber.Create(request.TraceNumber),
            TerminalId.Create(request.TerminalId),
            request.IdempotencyKey);
    }

    private async Task SaveTransactionAsync(
        PaymentTransaction transaction,
        CancellationToken cancellationToken)
    {
        await _repository.AddAsync(
            transaction,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    private async Task<BankPurchaseResponse> PurchaseFromBankAsync(
        CreatePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        return await _bankClient.PurchaseAsync(
            new BankPurchaseRequest(
                request.Pan,
                request.Amount,
                request.TerminalId,
                request.TraceNumber),
            cancellationToken);
    }

    private async Task<CreatePurchaseResponse> HandleFailedPurchaseAsync(
        PaymentTransaction transaction,
        CancellationToken cancellationToken)
    {
        transaction.Fail();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        _logger.LogWarning(
            "Purchase failed. Transaction:{TransactionId}",
            transaction.Id);

        return CreateResponse(transaction);
    }

    private async Task<CreatePurchaseResponse> HandleSuccessfulPurchaseAsync(
        PaymentTransaction transaction,
        string rrn,
        CreatePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        transaction.PurchaseSucceeded(rrn);

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
            "Purchase completed successfully. Transaction:{TransactionId}",
            transaction.Id);

        return CreateResponse(transaction);
    }

    private static CreatePurchaseResponse CreateResponse(
        PaymentTransaction transaction)
    {
        return new CreatePurchaseResponse(
            transaction.Id,
            transaction.Status.ToString());
    }
}
