using PSP.Topup.Domain.TopupAggregate;

namespace PSP.Topup.Application.Abstractions.Persistence;

public interface ITopupRepository
{
    Task AddAsync(
        TopupTransaction transaction,
        CancellationToken cancellationToken = default);

    Task<TopupTransaction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<TopupTransaction?> GetByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
