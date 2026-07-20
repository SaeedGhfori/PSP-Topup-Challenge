using PSP.Mock.Bank.Api.Extensions;

using Serilog;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console();
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddBankServices();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseGlobalException();

app.UseAuthorization();

app.MapControllers();

app.Run();
