using PSP.Entities;

namespace PSP.Abstractions.Persistence;

public interface ITopupRepository
{
    Task AddAsync(
        TopupTransaction transaction,
        CancellationToken cancellationToken);

    Task<TopupTransaction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<bool> ExistsByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}
