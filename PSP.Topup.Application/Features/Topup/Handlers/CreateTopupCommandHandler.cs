using MediatR;

using PSP.Features.Topup.Commands;
using PSP.Features.Topup.DTOs;

namespace PSP.Features.Topup.Handlers;

public sealed class CreateTopupCommandHandler
    : IRequestHandler<CreateTopupCommand, CreateTopupResponse>
{
    public Task<CreateTopupResponse> Handle(
        CreateTopupCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
