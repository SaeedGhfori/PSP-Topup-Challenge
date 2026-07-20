using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PSP.Payment.Domain.Entities;

namespace PSP.Payment.Persistence.Configurations;

public sealed class PaymentTransactionConfiguration
    : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(
        EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(11);

        builder.Property(x => x.OperatorId);

        builder.Property(x => x.IdempotencyKey)
            .HasMaxLength(100);

        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique();

        builder.Property(x => x.CreatedAtUtc);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.Type)
            .HasConversion<int>();

        builder.OwnsOne(x => x.Pan, pan =>
        {
            pan.Property(p => p.Value)
                .HasColumnName("Pan")
                .HasMaxLength(16);
        });

        builder.OwnsOne(x => x.Amount, amount =>
        {
            amount.Property(a => a.Value)
                .HasColumnName("Amount")
                .HasPrecision(18, 2);
        });

        builder.OwnsOne(x => x.TraceNumber, trace =>
        {
            trace.Property(t => t.Value)
                .HasColumnName("TraceNumber")
                .HasMaxLength(30);
        });

        builder.OwnsOne(x => x.TerminalId, terminal =>
        {
            terminal.Property(t => t.Value)
                .HasColumnName("TerminalId")
                .HasMaxLength(30);
        });
    }
}
