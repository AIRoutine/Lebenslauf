using Lebenslauf.Api.Core.Data;
using Lebenslauf.Api.Core.Data.Seeding;
using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lebenslauf.Api.Features.Cv.Data.Seeding;

/// <summary>
/// Seeds profile variants for the CV application.
/// Runs before CvSeeder (Order = 5) to ensure profiles exist.
/// </summary>
public class ProfileSeeder(AppDbContext dbContext) : ISeeder
{
    public int Order => 5; // Run before CvSeeder (Order = 10)

    // Static IDs for reference in other seeders
    public static readonly Guid DefaultProfileId = new("11111111-1111-1111-1111-111111111111");
    public static readonly Guid BackendProfileId = new("22222222-2222-2222-2222-222222222222");
    public static readonly Guid MobileProfileId = new("33333333-3333-3333-3333-333333333333");

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await dbContext.Set<Profile>().AnyAsync(cancellationToken))
            return;

        var profiles = new List<Profile>
        {
            new()
            {
                Id = DefaultProfileId,
                Slug = "default",
                Name = "Vollstaendiges Profil",
                Description = "Zeigt alle Skills und Projekte",
                IsDefault = true
            },
            new()
            {
                Id = BackendProfileId,
                Slug = "backend",
                Name = "Backend Developer",
                Description = "Fokus auf .NET, ASP.NET, APIs und Datenbanken",
                IsDefault = false
            },
            new()
            {
                Id = MobileProfileId,
                Slug = "mobile",
                Name = "Mobile Developer",
                Description = "Fokus auf Cross-Platform Mobile Entwicklung mit MAUI und Uno",
                IsDefault = false
            }
        };

        await dbContext.Set<Profile>().AddRangeAsync(profiles, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
