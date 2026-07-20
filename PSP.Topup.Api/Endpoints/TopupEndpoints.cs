using MediatR;

using Microsoft.AspNetCore.Mvc;

using PSP.Topup.Application.Features.Topup.Commands;

namespace PSP.Topup.Api.Endpoints;

public static class TopupEndpoints
{
    public static IEndpointRouteBuilder MapTopupEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/topups")
            .WithTags("Topup");

        group.MapPost("/", CreateTopup);

        return app;
    }

    private static async Task<IResult> CreateTopup(
        [FromBody] CreateTopupCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        Guid transactionId =
            await sender.Send(command, cancellationToken);

        return Microsoft.AspNetCore.Http.TypedResults.Created(
            $"/api/topups/{transactionId}",
            new
            {
                transactionId
            });
    }
}
