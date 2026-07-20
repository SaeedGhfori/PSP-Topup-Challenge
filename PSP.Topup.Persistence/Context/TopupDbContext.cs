using Microsoft.EntityFrameworkCore;

using PSP.Topup.Domain.TopupAggregate;

namespace PSP.Topup.Persistence.Context;

public sealed class TopupDbContext : DbContext
{
    public TopupDbContext(DbContextOptions<TopupDbContext> options)
        : base(options)
    {
    }

    public DbSet<TopupTransaction> TopupTransactions => Set<TopupTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TopupDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
