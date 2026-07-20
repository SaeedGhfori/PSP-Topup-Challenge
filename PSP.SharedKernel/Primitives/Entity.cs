namespace PSP.SharedKernel.Entities;

/// <summary>
/// Represents the base entity.
/// </summary>
public abstract class Entity
{
    private readonly List<object> _domainEvents = [];

    public Guid Id { get; protected set; }

    public IReadOnlyCollection<object> DomainEvents
        => _domainEvents.AsReadOnly();

    public void RaiseDomainEvent(object domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
