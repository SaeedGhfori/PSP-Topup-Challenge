namespace PSP.Topup.Application.Contracts.Services;

public interface ITopupProcessor
{
    Task ProcessAsync(
        Guid transactionId,
        CancellationToken cancellationToken);
}
