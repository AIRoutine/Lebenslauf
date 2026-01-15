namespace Lebenslauf.Api.Features.Cv.Data.Entities;

/// <summary>
/// Join table linking profiles to projects with profile-specific settings.
/// </summary>
public class ProfileProject
{
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    /// <summary>
    /// Sort order within this profile (overrides default project order).
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Whether this project should be highlighted in this profile.
    /// </summary>
    public bool IsHighlighted { get; set; }

    /// <summary>
    /// Optional description override for this profile.
    /// If set, this description is used instead of the default project description.
    /// </summary>
    public string? DescriptionOverride { get; set; }
}
