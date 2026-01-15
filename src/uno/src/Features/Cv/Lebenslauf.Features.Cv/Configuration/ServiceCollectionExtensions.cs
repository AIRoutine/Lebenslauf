using Lebenslauf.Features.Cv.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lebenslauf.Features.Cv.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCvFeature(this IServiceCollection services)
    {
        // Profile state management
        services.AddSingleton<IProfileStateService, ProfileStateService>();

        return services;
    }
}
