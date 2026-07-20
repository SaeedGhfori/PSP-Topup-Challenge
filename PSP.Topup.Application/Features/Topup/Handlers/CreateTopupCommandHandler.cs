using MediatR;

using PSP.Topup.Application.Features.Topup.Commands;
using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Entities;
using PSP.Topup.Domain.Enums;
using PSP.Topup.Domain.Repositories;

public sealed class CreateTopupCommandHandler
    : IRequestHandler<CreateTopupCommand, Guid>
{
    private readonly ITopupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTopupCommandHandler(
        ITopupRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateTopupCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.GetByIdempotencyKeyAsync(
            request.IdempotencyKey,
            cancellationToken);

        if (exists is not null)
            return exists.Id;

        var transaction = TopupTransaction.Create(
            PhoneNumber.Create(request.PhoneNumber),
            Money.Create(request.Amount),
            (MobileOperator)request.OperatorId,
            request.IdempotencyKey);

        await _repository.AddAsync(
            transaction,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return transaction.Id;
    }
}
