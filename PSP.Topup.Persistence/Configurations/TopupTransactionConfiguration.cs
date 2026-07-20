using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PSP.Topup.Domain.TopupAggregate;

namespace PSP.Topup.Persistence.Configurations;

public sealed class TopupTransactionConfiguration
    : IEntityTypeConfiguration<TopupTransaction>
{
    public void Configure(EntityTypeBuilder<TopupTransaction> builder)
    {
        builder.ToTable("topup_transactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(x => x.PhoneNumber, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("phone_number")
                .HasMaxLength(11)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Amount, money =>
        {
            money.Property(m => m.Value)
                .HasColumnName("amount")
                .HasPrecision(18, 2);
        });

        builder.Property(x => x.MobileOperator)
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.IdempotencyKey)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique();

        builder.Property(x => x.ProviderReference)
            .HasMaxLength(100);

        builder.Property(x => x.FailureReason)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc);

        builder.Property(x => x.DeletedAtUtc);

        builder.Property(x => x.IsDeleted);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
