using PSP.Topup.Domain.Common;

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
