using MediatR;

namespace PSP.Topup.Application.Features.Topup.Commands;

/// <summary>
/// Creates a new top-up transaction.
/// </summary>
public sealed record CreateTopupCommand(
    string PhoneNumber,
    decimal Amount,
    int OperatorId,
    string IdempotencyKey)
    : IRequest<Guid>;
