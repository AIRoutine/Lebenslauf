using Microsoft.Extensions.DependencyInjection;

namespace Lebenslauf.Features.Cv.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCvFeature(this IServiceCollection services)
    {
        // Feature services here if needed
        return services;
    }
}
