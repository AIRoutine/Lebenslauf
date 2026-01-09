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
    IReadOnlyList<ProjectModel> Projects
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
    IReadOnlyList<string> Technologies,
    string? AppStoreUrl,
    string? PlayStoreUrl,
    string? WebsiteUrl,
    string? ImageUrl
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
