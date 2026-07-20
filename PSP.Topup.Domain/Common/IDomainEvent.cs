namespace PSP.Topup.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
