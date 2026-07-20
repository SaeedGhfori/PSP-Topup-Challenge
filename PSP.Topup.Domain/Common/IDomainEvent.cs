namespace PSP.Topup.Domain.Common;

/// <summary>
/// Represents a domain event.
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }

    DateTime OccurredOnUtc { get; }
}
