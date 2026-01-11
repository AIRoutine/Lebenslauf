using Lebenslauf.Api.Core.Data;
using Lebenslauf.Api.Core.Data.Seeding;
using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lebenslauf.Api.Features.Cv.Data.Seeding;

/// <summary>
/// Seeds GitHub contribution data for the contribution graph.
/// </summary>
public class GitHubSeeder(AppDbContext dbContext) : ISeeder
{
    public int Order => 20; // After CvSeeder

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if already seeded
        if (await dbContext.Set<GitHubContribution>().AnyAsync(cancellationToken))
            return;

        var contributions = GenerateContributionsFor2025();

        await dbContext.Set<GitHubContribution>().AddRangeAsync(contributions, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static List<GitHubContribution> GenerateContributionsFor2025()
    {
        var contributions = new List<GitHubContribution>();
        var random = new Random(42); // Fixed seed for reproducibility

        // Generate data for January 2025 to January 2026 (like GitHub's "last year" view)
        var startDate = new DateOnly(2025, 1, 1);
        var endDate = new DateOnly(2026, 1, 11); // Current date

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            // Calculate week number (ISO week)
            var weekNumber = GetIsoWeekNumber(date);
            var dayOfWeek = (int)date.DayOfWeek;

            // Generate contribution count with realistic pattern
            // More contributions on weekdays, occasional busy days
            int count;

            // Weekends have fewer contributions
            if (dayOfWeek == 0 || dayOfWeek == 6)
            {
                count = random.Next(0, 8); // 0-7 contributions
            }
            else
            {
                // Weekdays - more active
                var baseContributions = random.Next(2, 15);

                // Some days are very productive (10% chance)
                if (random.NextDouble() < 0.10)
                {
                    count = random.Next(15, 35); // High activity day
                }
                // Some days have no activity (15% chance)
                else if (random.NextDouble() < 0.15)
                {
                    count = 0;
                }
                else
                {
                    count = baseContributions;
                }
            }

            contributions.Add(new GitHubContribution
            {
                Id = Guid.NewGuid(),
                Date = date,
                Count = count,
                WeekNumber = weekNumber,
                DayOfWeek = dayOfWeek
            });
        }

        return contributions;
    }

    private static int GetIsoWeekNumber(DateOnly date)
    {
        var dateTime = date.ToDateTime(TimeOnly.MinValue);
        var cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
        return cal.GetWeekOfYear(dateTime,
            System.Globalization.CalendarWeekRule.FirstFourDayWeek,
            System.DayOfWeek.Monday);
    }
}
