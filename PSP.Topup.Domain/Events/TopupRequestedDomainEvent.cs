using PSP.Topup.Domain.Common;

namespace PSP.Topup.Domain.Events;

/// <summary>
/// Raised when a topup transaction is created.
/// </summary>
public sealed record TopupRequestedDomainEvent(Guid TransactionId)
    : DomainEvent;
