using PSP.Topup.Api.Endpoints;
using PSP.Topup.Api.Extensions;
using PSP.Topup.Application;
using PSP.Topup.Infrastructure;
using PSP.Topup.Persistence;

using Scalar.AspNetCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseGlobalException();
app.MapTopupEndpoints();
app.MapGet("/", () => Results.Redirect("/scalar"));

app.Run();
