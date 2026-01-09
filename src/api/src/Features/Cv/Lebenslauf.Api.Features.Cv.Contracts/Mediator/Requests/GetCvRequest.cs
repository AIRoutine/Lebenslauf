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
    IReadOnlyList<EducationDto> Education,
    IReadOnlyList<WorkExperienceDto> WorkExperience,
    IReadOnlyList<SkillCategoryDto> SkillCategories,
    IReadOnlyList<ProjectDto> Projects
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
