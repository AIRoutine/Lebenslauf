using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

/// <summary>
/// Represents a profile variant that controls which data is displayed.
/// Each profile can show different skills, projects, and personal data
/// tailored for specific audiences (e.g., backend-dev, mobile-dev, consultant).
/// </summary>
public class Profile : BaseEntity
{
    /// <summary>
    /// URL-friendly identifier (e.g., "backend-dev", "mobile-dev").
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Display name for the profile.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Optional description of this profile variant.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// If true, this profile is used when no specific profile is requested.
    /// </summary>
    public bool IsDefault { get; set; }

    // Navigation properties
    public PersonalData? PersonalData { get; set; }
    public ICollection<ProfileSkill> ProfileSkills { get; set; } = [];
    public ICollection<ProfileProject> ProfileProjects { get; set; } = [];
    public ICollection<ProfileWorkExperience> ProfileWorkExperiences { get; set; } = [];
}
