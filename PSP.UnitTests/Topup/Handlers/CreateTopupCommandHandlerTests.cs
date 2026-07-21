

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using PSP.Contracts.Events;
using PSP.Messaging.Abstractions;
using PSP.Topup.Application.Features.Topup.Commands;
using PSP.Topup.Application.Features.Topup.Handlers;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Entities;
using PSP.Topup.Domain.Enums;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Handlers;

public class CreateTopupCommandHandlerTests
{
    private readonly Mock<ITopupRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMessageBus> _messageBusMock;
    private readonly Mock<ILogger<CreateTopupCommandHandler>> _loggerMock;
    private readonly CreateTopupCommandHandler _handler;

    public CreateTopupCommandHandlerTests()
    {
        _repositoryMock = new Mock<ITopupRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _messageBusMock = new Mock<IMessageBus>();
        _loggerMock = new Mock<ILogger<CreateTopupCommandHandler>>();

        _handler = new CreateTopupCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _messageBusMock.Object,
            _loggerMock.Object);
    }

    private CreateTopupCommand CreateValidCommand()
    {
        return new CreateTopupCommand(
            PhoneNumber: "09123456789",
            Amount: 50000,
            OperatorId: 1,
            IdempotencyKey: Guid.NewGuid().ToString()
        );
    }

    [Fact]
    public async Task Handle_WhenDuplicateExists_ShouldReturnExistingTransaction()
    {
        // Arrange
        var command = CreateValidCommand();
        var existingTransaction = TopupTransaction.Create(
            Guid.NewGuid(),
            PhoneNumber.Create(command.PhoneNumber),
            Money.Create(command.Amount),
            (MobileOperator)command.OperatorId,
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

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TopupTransaction>(), It.IsAny<CancellationToken>()), Times.Never);
        _messageBusMock.Verify(x => x.PublishAsync(It.IsAny<TopupRequestedIntegrationEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNewTransaction_ShouldCreateAndSaveTransaction()
    {
        // Arrange
        var command = CreateValidCommand();
        TopupTransaction? savedTransaction = null;

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(command.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TopupTransaction?)null);

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TopupTransaction>(), It.IsAny<CancellationToken>()))
            .Callback<TopupTransaction, CancellationToken>((tx, _) => savedTransaction = tx);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        savedTransaction.Should().NotBeNull();
        savedTransaction?.Status.Should().Be(TransactionStatus.Pending);

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TopupTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPublishIntegrationEvent()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(command.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TopupTransaction?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _messageBusMock.Verify(
            x => x.PublishAsync(
                It.Is<TopupRequestedIntegrationEvent>(e =>
                    e.PhoneNumber == command.PhoneNumber &&
                    e.Amount == command.Amount &&
                    e.OperatorId == command.OperatorId &&
                    e.IdempotencyKey == command.IdempotencyKey),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldValidatePhoneNumberValueObject()
    {
        // Arrange
        var invalidPhoneCommand = new CreateTopupCommand(
            PhoneNumber: "123", // Invalid phone number
            Amount: 50000,
            OperatorId: 1,
            IdempotencyKey: Guid.NewGuid().ToString()
        );

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TopupTransaction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(invalidPhoneCommand, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldValidateMoneyValueObject()
    {
        // Arrange
        var invalidAmountCommand = new CreateTopupCommand(
            PhoneNumber: "09123456789",
            Amount: -1000, // Invalid amount
            OperatorId: 1,
            IdempotencyKey: Guid.NewGuid().ToString()
        );

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TopupTransaction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            _handler.Handle(invalidAmountCommand, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateError()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock
            .Setup(x => x.GetByIdempotencyKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
