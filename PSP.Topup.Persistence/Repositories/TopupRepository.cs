using Microsoft.EntityFrameworkCore;
using PSP.Entities;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Persistence.Repositories;

public sealed class TopupRepository : ITopupRepository
{
    private readonly TopupDbContext _dbContext;

    public TopupRepository(TopupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        TopupTransaction transaction,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.TopupTransactions
            .AddAsync(transaction, cancellationToken);
    }

    public async Task<TopupTransaction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TopupTransactions
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<TopupTransaction?> GetByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TopupTransactions
            .FirstOrDefaultAsync(
                x => x.IdempotencyKey == idempotencyKey,
                cancellationToken);
    }
}
