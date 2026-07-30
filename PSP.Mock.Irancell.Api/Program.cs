using PSP.Mock.Irancell.Api.Extensions;

using Scalar.AspNetCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console();
});


builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddIrancellServices();


var app = builder.Build();


app.MapOpenApi();

app.MapScalarApiReference();

app.MapGet("/", () => Results.Redirect("/scalar"));

app.UseHttpsRedirection();


// Global Exception Middleware
app.UseGlobalException();


app.UseAuthorization();


app.MapControllers();


app.Run();
