using MediatR;

using PSP.Payment.Application.Features.DTOs;

namespace PSP.Payment.Application.Features.Payments.Commands;

public sealed record CreatePurchaseCommand(
    string Pan,
    decimal Amount,
    string PhoneNumber,
    int OperatorId,
    string TerminalId,
    string TraceNumber,
    string IdempotencyKey)
    : IRequest<CreatePurchaseResponse>;
