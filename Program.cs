using CarStockApi.Data;
using CarStockApi.Data.Repositories;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var jwtSigningKey = builder.Configuration["Jwt:SigningKey"]
    ?? throw new InvalidOperationException("JWT signing key is missing.");

builder.Services.AddAuthenticationJwtBearer(options =>
    {
        options.SigningKey = jwtSigningKey;
    })
    .AddAuthorization()
    .AddFastEndpoints();

builder.Services.Configure<JwtCreationOptions>(options =>
    {
        options.SigningKey = jwtSigningKey;
    });


builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<CarRepository>();
builder.Services.AddScoped<DealerRepository>();

var app = builder.Build();

var database = app.Services.GetRequiredService<Database>();
database.Initialize();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (exceptionFeature is not null)
        {
            app.Logger.LogError(
                exceptionFeature.Error,
                "An unhandled exception occurred.");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            message = "An unexpected error occurred."
        });
    });
});

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();