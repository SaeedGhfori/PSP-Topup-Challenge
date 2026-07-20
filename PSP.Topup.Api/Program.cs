using PSP.Topup.Api.Endpoints;
using PSP.Topup.Application;
using PSP.Topup.Persistence;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);


var app = builder.Build();

app.MapOpenApi();
app.MapTopupEndpoints();
app.MapGet("/", () => Results.Ok("PSP Topup Service"));

app.Run();
