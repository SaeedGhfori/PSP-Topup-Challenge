namespace PSP.Contracts.Messaging;

public interface IIntegrationEvent : IMessage
{
    Guid EventId { get; }

    DateTime OccurredOnUtc { get; }
}
