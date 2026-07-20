using PSP.Payment.Domain.Enums;
using PSP.Payment.Domain.ValueObjects;

namespace PSP.Payment.Domain.Entities;

public sealed class PaymentTransaction
{
    private PaymentTransaction()
    {
    }

    public Guid Id { get; private set; }

    public Pan Pan { get; private set; } = default!;

    public Money Amount { get; private set; } = default!;

    public string PhoneNumber { get; private set; } = default!;

    public int OperatorId { get; private set; }

    public TraceNumber TraceNumber { get; private set; } = default!;

    public TerminalId TerminalId { get; private set; } = default!;

    public PaymentType Type { get; private set; }

    public PaymentStatus Status { get; private set; }

    public string? Rrn { get; private set; }

    public string IdempotencyKey { get; private set; } = default!;

    public DateTime CreatedAtUtc { get; private set; }

    public static PaymentTransaction Create(
        Pan pan,
        Money amount,
        string phoneNumber,
        int operatorId,
        TraceNumber traceNumber,
        TerminalId terminalId,
        string idempotencyKey)
    {
        return new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            Pan = pan,
            Amount = amount,
            PhoneNumber = phoneNumber,
            OperatorId = operatorId,
            TraceNumber = traceNumber,
            TerminalId = terminalId,
            IdempotencyKey = idempotencyKey,
            CreatedAtUtc = DateTime.UtcNow,
            Status = PaymentStatus.Pending,
            Type = PaymentType.Topup
        };
    }

    public void PurchaseSucceeded(string rrn)
    {
        Status = PaymentStatus.Purchased;
        Rrn = rrn;
    }

    public void Confirm()
    {
        Status = PaymentStatus.Confirmed;
    }

    public void Reverse()
    {
        Status = PaymentStatus.Reversed;
    }

    public void Fail()
    {
        Status = PaymentStatus.Failed;
    }
}
