using MediatR;

using Microsoft.Extensions.Logging;

using PSP.Topup.Application.Contracts.Mci;
using PSP.Topup.Application.Features.Topup.Commands;
using PSP.Topup.Application.Features.Topup.DTOs;

using PSP.Topup.Domain.Common;
using PSP.Topup.Domain.Entities;
using PSP.Topup.Domain.Enums;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Application.Features.Topup.Handlers;

public sealed class CreateTopupCommandHandler
    : IRequestHandler<CreateTopupCommand, CreateTopupResponse>
{
    private readonly ITopupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMciClient _mciClient;
    private readonly ILogger<CreateTopupCommandHandler> _logger;

    public CreateTopupCommandHandler(
        ITopupRepository repository,
        IUnitOfWork unitOfWork,
        IMciClient mciClient,
        ILogger<CreateTopupCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mciClient = mciClient;
        _logger = logger;
    }

    public async Task<CreateTopupResponse> Handle(
        CreateTopupCommand request,
        CancellationToken cancellationToken)
    {
        // Idempotency
        var duplicate = await _repository.GetByIdempotencyKeyAsync(
            request.IdempotencyKey,
            cancellationToken);

        if (duplicate is not null)
        {
            _logger.LogInformation(
                "Duplicate request detected. IdempotencyKey: {Key}",
                request.IdempotencyKey);

            return new CreateTopupResponse(
                duplicate.Id,
                duplicate.Status.ToString());
        }

        var transaction = TopupTransaction.Create(
            PhoneNumber.Create(request.PhoneNumber),
            Money.Create(request.Amount),
            (MobileOperator)request.OperatorId,
            request.IdempotencyKey);

        await _repository.AddAsync(
            transaction,
            cancellationToken);

        var response = await _mciClient.TopupAsync(
            new MciTopupRequest(
                request.PhoneNumber,
                request.Amount,
                transaction.Id.ToString()),
            cancellationToken);

        if (response.Success)
        {
            transaction.MarkSucceeded(
                response.ReferenceNumber);
        }
        else
        {
            transaction.MarkFailed(
                response.Message);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        _logger.LogInformation(
            "Topup completed. TransactionId: {Id}, Status: {Status}",
            transaction.Id,
            transaction.Status);

        return new CreateTopupResponse(
            transaction.Id,
            transaction.Status.ToString());
    }
}
