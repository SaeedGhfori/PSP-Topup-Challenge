namespace PSP.Messaging.Contracts;

public interface IIntegrationEvent
{
    Guid EventId { get; }

    DateTime OccurredOnUtc { get; }
}
