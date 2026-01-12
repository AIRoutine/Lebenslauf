using Lebenslauf.Core.ApiClient.Configuration;
using Lebenslauf.Features.Cv.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Mediator.Infrastructure;
using UnoFramework.Mediator;
using UnoFramework.ViewModels;

namespace Lebenslauf.Core.Startup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // Auto-register services with [Service] attribute
        services.AddShinyServiceRegistry();

        // Note: HTTP handlers are automatically registered via source generation
        // when contracts implement IHttpRequest<T>
        services.AddShinyMediator();
        services.AddSingleton<IEventCollector, UnoEventCollector>();
        services.AddSingleton<BaseServices>();

        // Core
        services.AddApiClientFeature();

        // Features
        services.AddCvFeature();

        return services;
    }
}
