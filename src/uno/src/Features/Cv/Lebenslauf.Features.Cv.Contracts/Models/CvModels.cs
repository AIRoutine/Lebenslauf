namespace Lebenslauf.Features.Cv.Contracts.Models;

/// <summary>
/// Complete CV response with all sections.
/// </summary>
public record CvData(
    PersonalDataModel PersonalData,
    IReadOnlyList<FamilyMemberModel> FamilyMembers,
    IReadOnlyList<EducationModel> Education,
    IReadOnlyList<InternshipModel> Internships,
    IReadOnlyList<WorkExperienceModel> WorkExperience,
    IReadOnlyList<SkillCategoryModel> SkillCategories,
    IReadOnlyList<ProjectModel> Projects,
    GitHubContributionsModel GitHub
);

/// <summary>
/// Personal information.
/// </summary>
public record PersonalDataModel(
    string Name,
    string Title,
    string Email,
    string Phone,
    string Address,
    string City,
    string PostalCode,
    string Country,
    DateOnly BirthDate,
    string Citizenship,
    string? ProfileImageUrl
);

/// <summary>
/// Education entry.
/// </summary>
public record EducationModel(
    Guid Id,
    string Institution,
    string Degree,
    int StartYear,
    int? EndYear,
    string? Description
);

/// <summary>
/// Work experience entry.
/// </summary>
public record WorkExperienceModel(
    Guid Id,
    string Company,
    string Role,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description,
    bool IsCurrent
);

/// <summary>
/// Skill category with skills.
/// </summary>
public record SkillCategoryModel(
    Guid Id,
    string Name,
    IReadOnlyList<SkillModel> Skills
);

/// <summary>
/// Individual skill.
/// </summary>
public record SkillModel(
    Guid Id,
    string Name
);

/// <summary>
/// Project/reference entry.
/// </summary>
public record ProjectModel(
    Guid Id,
    string Name,
    string? Description,
    string? Framework,
    IReadOnlyList<string> Technologies,
    IReadOnlyList<string> Functions,
    IReadOnlyList<string> TechnicalAspects,
    IReadOnlyList<SubProjectModel> SubProjects,
    string? AppStoreUrl,
    string? PlayStoreUrl,
    string? AppGalleryUrl,
    string? WebsiteUrl,
    string? ImageUrl,
    DateOnly? StartDate,
    DateOnly? EndDate,
    bool IsCurrent
)
{
    /// <summary>
    /// Formatted timeline string (e.g., "Jan 2018 - Heute" or "Jan 2019 - Maerz 2020").
    /// Returns null if no start date is available.
    /// </summary>
    public string? TimelineDisplay
    {
        get
        {
            if (!StartDate.HasValue)
                return null;

            var monthNames = new[] { "", "Jan", "Feb", "Maerz", "Apr", "Mai", "Juni", "Juli", "Aug", "Sep", "Okt", "Nov", "Dez" };
            var start = $"{monthNames[StartDate.Value.Month]} {StartDate.Value.Year}";
            var end = IsCurrent ? "Heute" : EndDate.HasValue ? $"{monthNames[EndDate.Value.Month]} {EndDate.Value.Year}" : null;

            return end is null ? start : $"{start} - {end}";
        }
    }
}

/// <summary>
/// Sub-project within a main project.
/// </summary>
public record SubProjectModel(
    Guid Id,
    string Name,
    string? Description,
    string? Framework,
    IReadOnlyList<string> Technologies
);

/// <summary>
/// Family member (profession only, no names).
/// </summary>
public record FamilyMemberModel(
    Guid Id,
    string Relationship,
    string Profession,
    int? BirthYear
);

/// <summary>
/// Internship/Praktikum entry.
/// </summary>
public record InternshipModel(
    Guid Id,
    string Company,
    string Role,
    int Year,
    int? Month,
    int? EndMonth,
    string? Description
);

/// <summary>
/// Single contribution day for the contribution graph.
/// </summary>
public record GitHubContributionModel(
    DateOnly Date,
    int Count,
    int WeekNumber,
    int DayOfWeek
);

/// <summary>
/// GitHub contributions data for the contribution graph.
/// </summary>
public record GitHubContributionsModel(
    string Username,
    string ProfileUrl,
    int TotalContributions,
    IReadOnlyList<GitHubContributionModel> Contributions
);

/// <summary>
/// CV Profile for admin selection.
/// </summary>
public record ProfileModel(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    bool IsDefault
);
