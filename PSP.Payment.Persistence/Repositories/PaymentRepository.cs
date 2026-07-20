using Microsoft.EntityFrameworkCore;

using PSP.Payment.Domain.Entities;
using PSP.Payment.Domain.Repositories;
using PSP.Payment.Persistence.Context;

namespace PSP.Payment.Persistence.Repositories;

public sealed class PaymentRepository
    : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(
        PaymentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        PaymentTransaction transaction,
        CancellationToken cancellationToken)
    {
        await _context.PaymentTransactions.AddAsync(
            transaction,
            cancellationToken);
    }

    public async Task<PaymentTransaction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.PaymentTransactions
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<PaymentTransaction?> GetByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        return await _context.PaymentTransactions
            .FirstOrDefaultAsync(
                x => x.IdempotencyKey == idempotencyKey,
                cancellationToken);
    }
}
