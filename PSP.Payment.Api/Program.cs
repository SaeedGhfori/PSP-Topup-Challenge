using PSP.Messaging;
using PSP.Payment.Api.Endpoints;
using PSP.Payment.Api.Extensions;
using PSP.Payment.Application;
using PSP.Payment.Infrastructure;
using PSP.Payment.Persistence;

using Scalar.AspNetCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddMessaging(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddApplication();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseGlobalException();

app.MapPaymentEndpoints();

app.MapGet("/",
    () => Results.Ok("PSP Payment Service"));

app.Run();
