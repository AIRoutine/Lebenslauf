
using Lebenslauf.Api.Core.Data.Configuration;
using Lebenslauf.Api.Core.Startup;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddApiServices(builder.Configuration);

// Add CORS for GitHub Pages and custom domain
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "https://airoutine.github.io",
                "https://dotnetmaui.at",
                "https://www.dotnetmaui.at")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.Services.EnsureDatabaseCreated();
await app.RunSeedersAsync();

app.UseCors();
app.MapDefaultEndpoints();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
    _ = app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();
