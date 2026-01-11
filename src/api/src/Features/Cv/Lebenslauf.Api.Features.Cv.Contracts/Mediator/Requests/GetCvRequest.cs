using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;

/// <summary>
/// Request to get the complete CV data.
/// </summary>
public record GetCvRequest : IRequest<GetCvResponse>;

/// <summary>
/// Complete CV response with all sections.
/// </summary>
public record GetCvResponse(
    PersonalDataDto PersonalData,
    IReadOnlyList<FamilyMemberDto> FamilyMembers,
    IReadOnlyList<EducationDto> Education,
    IReadOnlyList<InternshipDto> Internships,
    IReadOnlyList<WorkExperienceDto> WorkExperience,
    IReadOnlyList<SkillCategoryDto> SkillCategories,
    IReadOnlyList<ProjectDto> Projects,
    GitHubContributionsDto GitHub
);

/// <summary>
/// Personal information.
/// </summary>
public record PersonalDataDto(
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
public record EducationDto(
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
public record WorkExperienceDto(
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
public record SkillCategoryDto(
    Guid Id,
    string Name,
    IReadOnlyList<SkillDto> Skills
);

/// <summary>
/// Individual skill.
/// </summary>
public record SkillDto(
    Guid Id,
    string Name
);

/// <summary>
/// Project/reference entry.
/// </summary>
public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    string? Framework,
    IReadOnlyList<string> Technologies,
    IReadOnlyList<string> Functions,
    IReadOnlyList<string> TechnicalAspects,
    string? AppStoreUrl,
    string? PlayStoreUrl,
    string? WebsiteUrl,
    string? ImageUrl
);

/// <summary>
/// Family member entry (profession only, no names).
/// </summary>
public record FamilyMemberDto(
    Guid Id,
    string Relationship,
    string Profession,
    int? BirthYear
);

/// <summary>
/// Internship/Praktikum entry.
/// </summary>
public record InternshipDto(
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
public record GitHubContributionDto(
    DateOnly Date,
    int Count,
    int WeekNumber,
    int DayOfWeek
);

/// <summary>
/// GitHub contributions data for the contribution graph.
/// </summary>
public record GitHubContributionsDto(
    string Username,
    string ProfileUrl,
    int TotalContributions,
    IReadOnlyList<GitHubContributionDto> Contributions
);
