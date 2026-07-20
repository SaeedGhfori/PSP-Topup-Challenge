using MediatR;

using Microsoft.AspNetCore.Mvc;

using PSP.Topup.Application.Features.Topup.Commands;
using PSP.Topup.Application.Features.Topup.DTOs;
using PSP.Topup.Contracts.Responses;

namespace PSP.Topup.Api.Endpoints;

public static class TopupEndpoints
{
    public static IEndpointRouteBuilder MapTopupEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/topups")
            .WithTags("Topup");

        group.MapPost(string.Empty, CreateTopup)
            .WithName("CreateTopup")
            .WithSummary("Creates a new topup transaction.")
            .WithDescription("Creates a mobile topup transaction.")
            .Produces<CreateTopupResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> CreateTopup(
        [FromBody] CreateTopupCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var transactionId =
            await sender.Send(command, cancellationToken);

        return TypedResults.Created(
            $"/api/topups/{transactionId}",
            new CreateTopupResponse(transactionId));
    }
}
