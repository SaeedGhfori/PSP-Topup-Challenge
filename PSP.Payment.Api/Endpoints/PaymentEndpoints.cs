using MediatR;

using Microsoft.AspNetCore.Mvc;

using PSP.Payment.Application.Features.DTOs;
using PSP.Payment.Application.Features.Payments.Commands;

namespace PSP.Payment.Api.Endpoints;

public static class PaymentEndpoints
{
    public static IEndpointRouteBuilder MapPaymentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/payments")
            .WithTags("Payments");

        group.MapPost(
            "/purchase",
            CreatePurchase);

        return app;
    }

    private static async Task<IResult> CreatePurchase(
        [FromBody] CreatePurchaseCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        CreatePurchaseResponse response =
            await sender.Send(
                command,
                cancellationToken);

        return TypedResults.Created(
            $"/api/payments/{response.TransactionId}",
            response);
    }
}
