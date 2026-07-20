using MediatR;

using PSP.Topup.Application.Features.Topup.DTOs;

namespace PSP.Topup.Application.Features.Topup.Commands;

public sealed record CreateTopupCommand(
    string PhoneNumber,
    decimal Amount,
    int OperatorId,
    string IdempotencyKey)
    : IRequest<CreateTopupResponse>;
