using PSP.Topup.Domain.Common;
using PSP.Topup.Persistence.Context;

namespace PSP.Topup.Persistence.UnitOfWorks;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TopupDbContext _dbContext;

    public UnitOfWork(TopupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
