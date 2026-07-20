using MediatR;

using PSP.Features.Topup.DTOs;

namespace PSP.Features.Topup.Commands;

public sealed record CreateTopupCommand(
    string PhoneNumber,
    decimal Amount,
    int Operator,
    string IdempotencyKey)
    : IRequest<CreateTopupResponse>;
