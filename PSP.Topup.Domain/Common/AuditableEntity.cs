namespace PSP.Common;

/// <summary>
/// Base auditable entity.
/// </summary>
public abstract class AuditableEntity<TId> : AggregateRoot<TId>
    where TId : notnull
{
    protected AuditableEntity(TId id)
        : base(id)
    {
        CreatedAtUtc = DateTime.UtcNow;
    }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public byte[] RowVersion { get; private set; } = [];

    public void MarkUpdated()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;

        MarkUpdated();
    }
}
