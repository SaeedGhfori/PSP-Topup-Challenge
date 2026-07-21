
using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using PSP.Contracts.Events;
using PSP.Messaging.Abstractions;
using PSP.Payment.Application.Contracts.Bank;
using PSP.Payment.Application.Features.Payments.Commands;
using PSP.Payment.Application.Features.Payments.Handlers;
using PSP.Payment.Domain.Common;
using PSP.Payment.Domain.Entities;
using PSP.Payment.Domain.Enums; 
using PSP.Payment.Domain.Repositories;
using PSP.Payment.Domain.ValueObjects;


namespace PSP.Payment.Handlers;

public class CreatePurchaseCommandHandlerTests
{
    private readonly Mock<IPaymentRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBankClient> _bankClientMock;
    private readonly Mock<IMessageBus> _messageBusMock;
    private readonly Mock<ILogger<CreatePurchaseCommandHandler>> _loggerMock;
    private readonly CreatePurchaseCommandHandler _handler;

    public CreatePurchaseCommandHandlerTests()
    {
        _repositoryMock = new Mock<IPaymentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _bankClientMock = new Mock<IBankClient>();
        _messageBusMock = new Mock<IMessageBus>();
        _loggerMock = new Mock<ILogger<CreatePurchaseCommandHandler>>();

        _handler = new CreatePurchaseCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _bankClientMock.Object,
            _messageBusMock.Object,
            _loggerMock.Object);
    }

    private CreatePurchaseCommand CreateValidCommand()
    {
        return new CreatePurchaseCommand(
            Pan: "1234567890123456",
            Amount: 100000,
            PhoneNumber: "09123456789",
            OperatorId: 1,
            TerminalId: "TERM001",
            TraceNumber: "123456",
            IdempotencyKey: Guid.NewGuid().ToString()
        );
    }

    [Fact]
    public async Task Handle_WhenDuplicateExists_ShouldReturnExistingTransaction()
    {
        // Arrange
        var command = CreateValidCommand();
        var existingTransaction = PaymentTransaction.Create(
            Pan.Create(command.Pan),
            PSP.Payment.Domain.ValueObjects.Money.Create(command.Amount),
            command.PhoneNumber,
            command.OperatorId,
            TraceNumber.Create(command.TraceNumber),
            TerminalId.Create(command.TerminalId),
            command.IdempotencyKey);

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(command.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTransaction);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TransactionId.Should().Be(existingTransaction.Id);
        result.Status.Should().Be(existingTransaction.Status.ToString());

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<PaymentTransaction>(), It.IsAny<CancellationToken>()), Times.Never);
        _bankClientMock.Verify(x => x.PurchaseAsync(It.IsAny<BankPurchaseRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNewTransaction_ShouldCreateAndSaveTransaction()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(command.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentTransaction?)null);

        _bankClientMock
            .Setup(x => x.PurchaseAsync(It.IsAny<BankPurchaseRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BankPurchaseResponse(
                Success: true,
                Rrn: "RRN123456",
                ResponseCode: 0,
                Message: "Success"));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<PaymentTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        _bankClientMock.Verify(x => x.PurchaseAsync(It.IsAny<BankPurchaseRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenBankPurchaseFails_ShouldMarkTransactionAsFailed()
    {
        // Arrange
        var command = CreateValidCommand();
        PaymentTransaction? savedTransaction = null;

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(command.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentTransaction?)null);

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentTransaction>(), It.IsAny<CancellationToken>()))
            .Callback<PaymentTransaction, CancellationToken>((tx, _) => savedTransaction = tx);

        _bankClientMock
            .Setup(x => x.PurchaseAsync(It.IsAny<BankPurchaseRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BankPurchaseResponse(
                Success: false,
                Rrn: null!,
                ResponseCode: 1001,
                Message: "Insufficient funds"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedTransaction.Should().NotBeNull();
        savedTransaction?.Status.Should().Be(PaymentStatus.Failed);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        _messageBusMock.Verify(x => x.PublishAsync(It.IsAny<TopupRequestedIntegrationEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenBankPurchaseSucceeds_ShouldPublishTopupEvent()
    {
        // Arrange
        var command = CreateValidCommand();
        PaymentTransaction? savedTransaction = null;

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(command.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentTransaction?)null);

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentTransaction>(), It.IsAny<CancellationToken>()))
            .Callback<PaymentTransaction, CancellationToken>((tx, _) => savedTransaction = tx);

        _bankClientMock
            .Setup(x => x.PurchaseAsync(It.IsAny<BankPurchaseRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BankPurchaseResponse(
                Success: true,
                Rrn: "RRN123456",
                ResponseCode: 0,
                Message: "Success"));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedTransaction.Should().NotBeNull();
        savedTransaction?.Status.Should().Be(PaymentStatus.Purchased);

        _messageBusMock.Verify(
            x => x.PublishAsync(
                It.Is<TopupRequestedIntegrationEvent>(e =>
                    e.PhoneNumber == command.PhoneNumber &&
                    e.Amount == command.Amount),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldValidatePanValueObject()
    {
        // Arrange
        var invalidPanCommand = new CreatePurchaseCommand(
            Pan: "123",
            Amount: 100000,
            PhoneNumber: "09123456789",
            OperatorId: 1,
            TerminalId: "TERM001",
            TraceNumber: "123456",
            IdempotencyKey: Guid.NewGuid().ToString()
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(invalidPanCommand, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldValidateMoneyValueObject()
    {
        // Arrange
        var invalidAmountCommand = new CreatePurchaseCommand(
            Pan: "1234567890123456",
            Amount: -1000,
            PhoneNumber: "09123456789",
            OperatorId: 1,
            TerminalId: "TERM001",
            TraceNumber: "123456",
            IdempotencyKey: Guid.NewGuid().ToString()
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            _handler.Handle(invalidAmountCommand, CancellationToken.None));
    }
}
