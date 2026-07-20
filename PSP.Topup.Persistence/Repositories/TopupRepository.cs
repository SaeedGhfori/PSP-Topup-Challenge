using Microsoft.EntityFrameworkCore;

using PSP.Topup.Domain.Entities;
using PSP.Topup.Domain.Repositories;
using PSP.Topup.Persistence.Context;

namespace PSP.Topup.Persistence.Repositories;

public sealed class TopupRepository : ITopupRepository
{
    private readonly TopupDbContext _context;

    public TopupRepository(TopupDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
    TopupTransaction transaction,
    CancellationToken cancellationToken = default)
    {
        await _context.TopupTransactions.AddAsync(transaction, cancellationToken);
    }

    public async Task<TopupTransaction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.TopupTransactions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TopupTransaction?> GetByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        return await _context.TopupTransactions
            .FirstOrDefaultAsync(
                x => x.IdempotencyKey == idempotencyKey,
                cancellationToken);
    }
}
