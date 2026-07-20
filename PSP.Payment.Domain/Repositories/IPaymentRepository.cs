using PSP.Payment.Domain.Entities;

namespace PSP.Payment.Domain.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(
        PaymentTransaction transaction,
        CancellationToken cancellationToken);

    Task<PaymentTransaction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PaymentTransaction?> GetByIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken cancellationToken);
}
