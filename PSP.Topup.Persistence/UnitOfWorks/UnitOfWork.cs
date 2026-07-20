using PSP.Topup.Domain.Common;
using PSP.Topup.Persistence.Context;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TopupDbContext _context;

    public UnitOfWork(TopupDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
