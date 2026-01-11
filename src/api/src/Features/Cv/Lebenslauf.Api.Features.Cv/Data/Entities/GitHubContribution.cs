using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

/// <summary>
/// Daily GitHub contribution data for the contribution graph.
/// </summary>
public class GitHubContribution : BaseEntity
{
    /// <summary>
    /// Date of the contribution.
    /// </summary>
    public required DateOnly Date { get; set; }

    /// <summary>
    /// Number of contributions on this date.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Week number within the year (1-52).
    /// </summary>
    public int WeekNumber { get; set; }

    /// <summary>
    /// Day of week (0 = Sunday, 6 = Saturday).
    /// </summary>
    public int DayOfWeek { get; set; }
}
