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

        // Target: 5,289 total contributions (including private)
        // Days: 376 -> ~14 contributions per day on average
        const int targetTotal = 5289;

        var tempContributions = new List<(DateOnly date, int weekNumber, int dayOfWeek)>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var weekNumber = GetIsoWeekNumber(date);
            var dayOfWeek = (int)date.DayOfWeek;
            tempContributions.Add((date, weekNumber, dayOfWeek));
        }

        // Generate base counts - more realistic distribution like real GitHub
        // Many days with 0 or low contributions, occasional high days
        var counts = new int[tempContributions.Count];
        for (var i = 0; i < tempContributions.Count; i++)
        {
            var dayOfWeek = tempContributions[i].dayOfWeek;

            // Weekends - often no activity
            if (dayOfWeek == 0 || dayOfWeek == 6)
            {
                var roll = random.NextDouble();
                if (roll < 0.40) // 40% no activity
                    counts[i] = 0;
                else if (roll < 0.70) // 30% low activity
                    counts[i] = random.Next(1, 4);
                else if (roll < 0.90) // 20% medium
                    counts[i] = random.Next(4, 10);
                else // 10% high
                    counts[i] = random.Next(10, 20);
            }
            else
            {
                // Weekdays - variable activity
                var roll = random.NextDouble();
                if (roll < 0.15) // 15% no activity
                    counts[i] = 0;
                else if (roll < 0.40) // 25% low activity (1-4)
                    counts[i] = random.Next(1, 5);
                else if (roll < 0.70) // 30% medium activity (5-12)
                    counts[i] = random.Next(5, 13);
                else if (roll < 0.90) // 20% high activity (13-25)
                    counts[i] = random.Next(13, 26);
                else // 10% very high activity (26-50)
                    counts[i] = random.Next(26, 51);
            }
        }

        // Scale to target total
        var currentTotal = counts.Sum();
        var scale = (double)targetTotal / currentTotal;
        for (var i = 0; i < counts.Length; i++)
        {
            counts[i] = Math.Max(0, (int)Math.Round(counts[i] * scale));
        }

        // Fine-tune to hit exact target
        var diff = targetTotal - counts.Sum();
        while (diff != 0)
        {
            var idx = random.Next(counts.Length);
            if (diff > 0)
            {
                counts[idx]++;
                diff--;
            }
            else if (counts[idx] > 0)
            {
                counts[idx]--;
                diff++;
            }
        }

        // Create contribution entities
        for (var i = 0; i < tempContributions.Count; i++)
        {
            var (date, weekNumber, dayOfWeek) = tempContributions[i];
            contributions.Add(new GitHubContribution
            {
                Id = Guid.NewGuid(),
                Date = date,
                Count = counts[i],
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
