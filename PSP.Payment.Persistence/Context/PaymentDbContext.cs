using Microsoft.EntityFrameworkCore;

using PSP.Payment.Domain.Entities;

namespace PSP.Payment.Persistence.Context;

public sealed class PaymentDbContext : DbContext
{
    public PaymentDbContext(
        DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }

    public DbSet<PaymentTransaction> PaymentTransactions
        => Set<PaymentTransaction>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PaymentDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
