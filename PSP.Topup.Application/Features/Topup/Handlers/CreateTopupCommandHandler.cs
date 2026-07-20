using MediatR;

namespace PSP.Topup.Application.Features.Topup.Create;

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
