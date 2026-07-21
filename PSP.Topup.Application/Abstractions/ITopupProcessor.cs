namespace PSP.Topup.Application.Abstractions;

public interface ITopupProcessor
{
    Task ProcessAsync(
        Guid transactionId,
        CancellationToken cancellationToken);
}
