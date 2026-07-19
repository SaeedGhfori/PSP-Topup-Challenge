namespace PSP.SharedKernel.Primitives;

public abstract class Entity
{
    private readonly List<object> _domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; protected set; }

    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(object domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }

        return GetType() == other.GetType() && Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right)
        => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right)
        => !Equals(left, right);
}
