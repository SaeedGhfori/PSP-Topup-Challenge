using MediatR;

using Microsoft.AspNetCore.Mvc;

using PSP.Topup.Application.Features.Topup.Commands;
using PSP.Topup.Application.Features.Topup.DTOs;

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
}
