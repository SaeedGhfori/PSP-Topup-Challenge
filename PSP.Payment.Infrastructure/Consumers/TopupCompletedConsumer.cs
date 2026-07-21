using MassTransit;

using Microsoft.Extensions.Logging;

using PSP.Contracts.Events;
using PSP.Payment.Application.Contracts.Bank;
using PSP.Payment.Domain.Common;
using PSP.Payment.Domain.Repositories;

namespace PSP.Payment.Infrastructure.Consumers;

public sealed class TopupCompletedConsumer
    : IConsumer<TopupCompletedIntegrationEvent>
{
    private readonly IPaymentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBankClient _bankClient;
    private readonly ILogger<TopupCompletedConsumer> _logger;

    public TopupCompletedConsumer(
        IPaymentRepository repository,
        IUnitOfWork unitOfWork,
        IBankClient bankClient,
        ILogger<TopupCompletedConsumer> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _bankClient = bankClient;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<TopupCompletedIntegrationEvent> context)
    {
        var message = context.Message;

        var payment =
            await _repository.GetByIdAsync(
                message.TransactionId,
                context.CancellationToken);

        if (payment is null)
        {
            _logger.LogWarning(
                "Payment transaction not found {Id}",
                message.TransactionId);

            return;
        }

        if (message.Success)
        {
            await _bankClient.ConfirmationAsync(
                new BankConfirmationRequest(payment.Rrn!),
                context.CancellationToken);

            payment.Confirm();

            _logger.LogInformation(
                "Bank Confirmation sent. {Id}",
                payment.Id);
        }
        else
        {
            await _bankClient.ReversalAsync(
                new BankReversalRequest(payment.Rrn!),
                context.CancellationToken);

            payment.Reverse();

            _logger.LogInformation(
                "Bank Reversal sent. {Id}",
                payment.Id);
        }

        await _unitOfWork.SaveChangesAsync(
            context.CancellationToken);
    }
}
