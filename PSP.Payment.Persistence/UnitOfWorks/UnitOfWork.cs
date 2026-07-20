using PSP.Payment.Domain.Common;
using PSP.Payment.Persistence.Context;

namespace PSP.Payment.Persistence;

public sealed class UnitOfWork
    : IUnitOfWork
{
    private readonly PaymentDbContext _context;

    public UnitOfWork(
        PaymentDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
