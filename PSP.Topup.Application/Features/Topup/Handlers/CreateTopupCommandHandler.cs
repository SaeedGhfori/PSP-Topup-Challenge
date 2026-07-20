using MediatR;

using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Application.Commands.CreateTopup;

public sealed class CreateTopupCommandHandler
    : IRequestHandler<CreateTopupCommand, Guid>
{
    private readonly ITopupRepository _repository;

    public CreateTopupCommandHandler(
        ITopupRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreateTopupCommand request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        throw new NotImplementedException();
    }
}
