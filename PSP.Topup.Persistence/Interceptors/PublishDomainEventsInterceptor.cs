using Microsoft.EntityFrameworkCore.Diagnostics;

using PSP.Topup.Domain.Common;

namespace PSP.Topup.Persistence.Interceptors;

public sealed class PublishDomainEventsInterceptor
    : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _dispatcher;

    public PublishDomainEventsInterceptor(
        IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return result;

        var domainEvents = eventData.Context
            .ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var aggregate in eventData.Context
                     .ChangeTracker
                     .Entries<AggregateRoot<Guid>>())
        {
            aggregate.Entity.ClearDomainEvents();
        }

        if (domainEvents.Count != 0)
        {
            await _dispatcher.DispatchAsync(
                domainEvents,
                cancellationToken);
        }

        return result;
    }
}
