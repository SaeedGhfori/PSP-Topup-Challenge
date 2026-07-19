namespace PSP.SharedKernel.Events;

public abstract record DomainEvent(
    Guid EventId,
    DateTime OccurredOnUtc)
{
    protected DomainEvent()
        : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}
