using System;

using FluentAssertions;

using PSP.Topup.Domain.Entities;
using PSP.Topup.Domain.Enums;

using Xunit;

namespace PSP.Topup.Domain;

public class TopupTransactionTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTransaction()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var phoneNumber = PhoneNumber.Create("09123456789");
        var money = Money.Create(50000);
        var mobileOperator = MobileOperator.Mci;
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act
        var transaction = TopupTransaction.Create(
            paymentId,
            phoneNumber,
            money,
            mobileOperator,
            idempotencyKey);

        // Assert
        transaction.Should().NotBeNull();
        transaction.Id.Should().NotBeEmpty();
        transaction.PaymentTransactionId.Should().Be(paymentId);
        transaction.PhoneNumber.Should().Be(phoneNumber);
        transaction.Amount.Should().Be(money);
        transaction.MobileOperator.Should().Be(mobileOperator);
        transaction.IdempotencyKey.Should().Be(idempotencyKey);
        transaction.Status.Should().Be(TransactionStatus.Pending);
        transaction.ProviderReference.Should().BeNull();
        transaction.FailureReason.Should().BeNull();
    }

    [Fact]
    public void MarkSucceeded_ShouldUpdateStatusAndProviderReference()
    {
        // Arrange
        var transaction = CreateValidTransaction();
        var providerReference = "REF123456";

        // Act
        transaction.MarkSucceeded(providerReference);

        // Assert
        transaction.Status.Should().Be(TransactionStatus.TopupSucceeded);
        transaction.ProviderReference.Should().Be(providerReference);
        transaction.FailureReason.Should().BeNull();
    }

    [Fact]
    public void MarkSucceeded_WithNullOrEmptyReference_ShouldThrowException()
    {
        // Arrange
        var transaction = CreateValidTransaction();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => transaction.MarkSucceeded(""));
        Assert.Throws<ArgumentNullException>(() => transaction.MarkSucceeded(null!));
    }

    [Fact]
    public void MarkFailed_ShouldUpdateStatusAndFailureReason()
    {
        // Arrange
        var transaction = CreateValidTransaction();
        var failureReason = "Insufficient balance";

        // Act
        transaction.MarkFailed(failureReason);

        // Assert
        transaction.Status.Should().Be(TransactionStatus.Failed);
        transaction.FailureReason.Should().Be(failureReason);
        transaction.ProviderReference.Should().BeNull();
    }

    [Fact]
    public void MarkFailed_WithNullOrEmptyReason_ShouldThrowException()
    {
        // Arrange
        var transaction = CreateValidTransaction();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => transaction.MarkFailed(""));
        Assert.Throws<ArgumentNullException>(() => transaction.MarkFailed(null!));
    }

    [Fact]
    public void MarkConfirmationSent_ShouldUpdateStatus()
    {
        // Arrange
        var transaction = CreateValidTransaction();

        // Act
        transaction.MarkConfirmationSent();

        // Assert
        transaction.Status.Should().Be(TransactionStatus.ConfirmationSent);
    }

    [Fact]
    public void MarkReversed_ShouldUpdateStatusAndFailureReason()
    {
        // Arrange
        var transaction = CreateValidTransaction();
        var reason = "Reversed due to timeout";

        // Act
        transaction.MarkReversed(reason);

        // Assert
        transaction.Status.Should().Be(TransactionStatus.ReversalSent);
        transaction.FailureReason.Should().Be(reason);
    }

    [Fact]
    public void MarkReversed_WithNullOrEmptyReason_ShouldThrowException()
    {
        // Arrange
        var transaction = CreateValidTransaction();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => transaction.MarkReversed(""));
        Assert.Throws<ArgumentNullException>(() => transaction.MarkReversed(null!));
    }

    private TopupTransaction CreateValidTransaction()
    {
        return TopupTransaction.Create(
            Guid.NewGuid(),
            PhoneNumber.Create("09123456789"),
            Money.Create(50000),
            MobileOperator.Mci,
            Guid.NewGuid().ToString());
    }
}
