using PSP.Common;

namespace PSP.Events;

/// <summary>
/// Raised when a topup transaction is created.
/// </summary>
public sealed record TopupRequestedDomainEvent(Guid TransactionId)
    : DomainEvent;
