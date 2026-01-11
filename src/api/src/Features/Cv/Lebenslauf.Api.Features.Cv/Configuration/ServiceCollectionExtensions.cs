using Lebenslauf.Api.Core.Data.Seeding.Configuration;
using Lebenslauf.Api.Features.Cv.Data.Seeding;
using Microsoft.Extensions.DependencyInjection;

namespace Lebenslauf.Api.Features.Cv.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCvFeature(this IServiceCollection services)
    {
        services.AddSeeder<CvSeeder>();
        services.AddSeeder<GitHubSeeder>();
        return services;
    }
}
