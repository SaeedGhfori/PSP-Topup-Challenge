using System;

using FluentAssertions;

using PSP.Payment.Domain.Entities;
using PSP.Payment.Domain.Enums;
using PSP.Payment.Domain.ValueObjects;
using Xunit;

namespace PSP.Payment.Domain;

public class PaymentTransactionTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTransaction()
    {
        // Arrange
        var pan = Pan.Create("1234567890123456");
        var money = PSP.Payment.Domain.ValueObjects.Money.Create(100000);
        var traceNumber = TraceNumber.Create("123456");
        var terminalId = TerminalId.Create("TERM001");
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act
        var transaction = PaymentTransaction.Create(
            pan,
            money,
            "09123456789",
            1,
            traceNumber,
            terminalId,
            idempotencyKey);

        // Assert
        transaction.Should().NotBeNull();
        transaction.Id.Should().NotBeEmpty();
        transaction.Status.Should().Be(PaymentStatus.Pending);
        transaction.Type.Should().Be(PaymentType.Topup);
        transaction.Pan.Should().Be(pan);
        transaction.Amount.Should().Be(money);
        transaction.TraceNumber.Should().Be(traceNumber);
        transaction.TerminalId.Should().Be(terminalId);
        transaction.IdempotencyKey.Should().Be(idempotencyKey);
        transaction.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void PurchaseSucceeded_ShouldUpdateStatusAndRrn()
    {
        // Arrange
        var transaction = CreateValidTransaction();
        var rrn = "RRN123456";

        // Act
        transaction.PurchaseSucceeded(rrn);

        // Assert
        transaction.Status.Should().Be(PaymentStatus.Purchased);
        transaction.Rrn.Should().Be(rrn);
    }

    [Fact]
    public void Confirm_ShouldUpdateStatusToConfirmed()
    {
        // Arrange
        var transaction = CreateValidTransaction();
        transaction.PurchaseSucceeded("RRN123");

        // Act
        transaction.Confirm();

        // Assert
        transaction.Status.Should().Be(PaymentStatus.Confirmed);
    }

    [Fact]
    public void Reverse_ShouldUpdateStatusToReversed()
    {
        // Arrange
        var transaction = CreateValidTransaction();

        // Act
        transaction.Reverse();

        // Assert
        transaction.Status.Should().Be(PaymentStatus.Reversed);
    }

    [Fact]
    public void Fail_ShouldUpdateStatusToFailed()
    {
        // Arrange
        var transaction = CreateValidTransaction();

        // Act
        transaction.Fail();

        // Assert
        transaction.Status.Should().Be(PaymentStatus.Failed);
    }

    private PaymentTransaction CreateValidTransaction()
    {
        return PaymentTransaction.Create(
            Pan.Create("1234567890123456"),
            PSP.Payment.Domain.ValueObjects.Money.Create(100000),
            "09123456789",
            1,
            TraceNumber.Create("123456"),
            TerminalId.Create("TERM001"),
            Guid.NewGuid().ToString());
    }
}
