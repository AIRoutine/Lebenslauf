using Microsoft.Extensions.DependencyInjection;

namespace Lebenslauf.Core.ApiClient.Configuration;

/// <summary>
/// Extension methods for registering ApiClient services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the ApiClient feature services.
    /// Generated HTTP contracts are automatically available via Shiny.Mediator.Http.
    /// </summary>
    public static IServiceCollection AddApiClientFeature(this IServiceCollection services)
    {
        // Shiny.Mediator.Http Handler werden automatisch durch das Package registriert
        // Die generierten Contracts (IHttpRequest<T>) werden vom HttpRequestHandler verarbeitet
        return services;
    }
}
