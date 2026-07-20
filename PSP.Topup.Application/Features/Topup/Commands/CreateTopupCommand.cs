using MediatR;

namespace PSP.Topup.Application.Features.Topup.Create;

public sealed record CreateTopupCommand(
    string PhoneNumber,
    decimal Amount,
    int Operator,
    string IdempotencyKey)
    : IRequest<CreateTopupResponse>;
