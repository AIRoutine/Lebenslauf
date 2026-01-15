namespace Lebenslauf.Api.Features.Cv.Data.Entities;

/// <summary>
/// Join table linking profiles to skills with profile-specific settings.
/// </summary>
public class ProfileSkill
{
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;

    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;

    /// <summary>
    /// Sort order within this profile (overrides default skill order).
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Whether this skill should be highlighted in this profile.
    /// </summary>
    public bool IsHighlighted { get; set; }
}
