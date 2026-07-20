using MediatR;

using Microsoft.AspNetCore.Mvc;

using PSP.Topup.Application.Features.Topup.Commands;
using PSP.Topup.Application.Features.Topup.DTOs;
using PSP.Topup.Domain.Repositories;

namespace PSP.Topup.Api.Endpoints;

public static class TopupEndpoints
{
    public static IEndpointRouteBuilder MapTopupEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/topups")
            .WithTags("Topup");

        group.MapPost("/", CreateTopup)
            .WithName("CreateTopup")
            .Produces<CreateTopupResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        group.MapGet("/{id:guid}", GetById)
            .WithName("GetTopupById")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> CreateTopup(
        [FromBody] CreateTopupCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response =
            await sender.Send(command, cancellationToken);

        return TypedResults.Created(
            $"/api/topups/{response.TransactionId}",
            response);
    }

    private static async Task<IResult> GetById(
    Guid id,
    ITopupRepository repository,
    CancellationToken cancellationToken)
    {
        var transaction =
            await repository.GetByIdAsync(
                id,
                cancellationToken);

        if (transaction is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(new
        {
            transaction.Id,
            PhoneNumber = transaction.PhoneNumber.Value,
            Amount = transaction.Amount.Value,
            transaction.Status,
            transaction.ProviderReference,
            transaction.FailureReason,
            transaction.CreatedAtUtc
        });
    }
}
