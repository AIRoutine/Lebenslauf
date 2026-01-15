namespace Lebenslauf.Api.Features.Cv.Data.Entities;

/// <summary>
/// Join table linking profiles to work experiences.
/// Allows different profiles to show different work experiences with custom ordering.
/// </summary>
public class ProfileWorkExperience
{
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;

    public Guid WorkExperienceId { get; set; }
    public WorkExperience WorkExperience { get; set; } = null!;

    /// <summary>
    /// Sort order within this profile (allows different ordering per profile).
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Whether this work experience should be highlighted in this profile.
    /// </summary>
    public bool IsHighlighted { get; set; }

    /// <summary>
    /// Optional description override for this profile.
    /// If set, this description is used instead of the default work experience description.
    /// </summary>
    public string? DescriptionOverride { get; set; }
}
