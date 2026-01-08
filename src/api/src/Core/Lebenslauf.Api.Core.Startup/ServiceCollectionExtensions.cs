using Lebenslauf.Api.Core.Data.Configuration;
using Lebenslauf.Api.Core.Data.Seeding;
using Lebenslauf.Api.Core.Data.Seeding.Configuration;
using Lebenslauf.Api.Features.Cv.Configuration;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shiny.Mediator;

namespace Lebenslauf.Api.Core.Startup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Data (must be before features to register DbContext first)
        services.AddAppData(configuration);
        services.AddDataSeeding();

        services.AddShinyServiceRegistry();
        services.AddShinyMediator();

        // Features
        services.AddCvFeature();

        return services;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // CV Endpoint
        app.MapGet("/api/cv", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Request(new GetCvRequest(), cancellationToken);
            return Results.Ok(response);
        })
        .WithName("GetCv")
        .WithTags("CV");

        return app;
    }

    public static async Task RunSeedersAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seederRunner = scope.ServiceProvider.GetRequiredService<SeederRunner>();
        await seederRunner.RunAllSeedersAsync();
    }
}