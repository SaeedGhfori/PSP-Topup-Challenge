using PSP.Topup.Domain.Entities;

namespace PSP.Topup.Domain.Repositories;

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
}
