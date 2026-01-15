using Lebenslauf.Api;
using Lebenslauf.Api.Core.Data;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Handlers;

[Service(ApiService.Lifetime, TryAdd = ApiService.TryAdd)]
public class GetCvHandler(AppDbContext dbContext) : IRequestHandler<GetCvRequest, GetCvResponse>
{
    [MediatorHttpGet("/api/cv/{ProfileSlug?}", OperationId = "GetCv")]
    public async Task<GetCvResponse> Handle(GetCvRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        // Get profile by slug, or default profile if not specified
        var profile = await GetProfileAsync(request.ProfileSlug, cancellationToken);
        var profileId = profile?.Id;

        // PersonalData is filtered by profile
        var personalData = profileId.HasValue
            ? await dbContext.Set<PersonalData>()
                .FirstOrDefaultAsync(x => x.ProfileId == profileId.Value, cancellationToken)
            : await dbContext.Set<PersonalData>()
                .FirstOrDefaultAsync(cancellationToken);

        // These are shared across all profiles
        var familyMembers = await dbContext.Set<FamilyMember>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var education = await dbContext.Set<Education>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var internships = await dbContext.Set<Internship>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        // Get profile-specific work experiences with highlighting and description overrides
        var (workExperiences, workExpInfo) = await GetWorkExperiencesForProfileAsync(profileId, cancellationToken);

        // Get profile-specific skills with highlighting info
        var (skillCategories, profileSkillInfo) = await GetSkillCategoriesForProfileAsync(profileId, cancellationToken);

        // Get profile-specific projects with highlighting and description overrides
        var (projects, projectInfo) = await GetProjectsForProfileAsync(profileId, cancellationToken);

        var gitHubContributions = await dbContext.Set<GitHubContribution>()
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);

        return new GetCvResponse(
            PersonalData: MapPersonalData(personalData),
            FamilyMembers: familyMembers.Select(MapFamilyMember).ToList(),
            Education: education.Select(MapEducation).ToList(),
            Internships: internships.Select(MapInternship).ToList(),
            WorkExperience: workExperiences.Select(w => MapWorkExperience(w, workExpInfo)).ToList(),
            SkillCategories: skillCategories.Select(c => MapSkillCategory(c, profileSkillInfo)).ToList(),
            Projects: projects.Select(p => MapProject(p, projectInfo)).ToList(),
            GitHub: MapGitHubContributions(gitHubContributions)
        );
    }

    private async Task<Profile?> GetProfileAsync(string? slug, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            // Return default profile
            return await dbContext.Set<Profile>()
                .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);
        }

        return await dbContext.Set<Profile>()
            .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    }

    private async Task<(List<SkillCategory> Categories, Dictionary<Guid, bool> HighlightInfo)> GetSkillCategoriesForProfileAsync(
        Guid? profileId, CancellationToken cancellationToken)
    {
        if (!profileId.HasValue)
        {
            // No profile filtering - return all skills
            var allCategories = await dbContext.Set<SkillCategory>()
                .Include(x => x.Skills.OrderBy(s => s.SortOrder))
                .OrderBy(x => x.SortOrder)
                .ToListAsync(cancellationToken);
            return (allCategories, new Dictionary<Guid, bool>());
        }

        // Get profile's skills with their highlight info
        var profileSkills = await dbContext.Set<ProfileSkill>()
            .Where(x => x.ProfileId == profileId.Value)
            .ToListAsync(cancellationToken);

        var profileSkillIds = profileSkills.Select(x => x.SkillId).ToHashSet();
        var highlightInfo = profileSkills.ToDictionary(x => x.SkillId, x => x.IsHighlighted);

        // Get categories and filter to only include skills in the profile
        var categories = await dbContext.Set<SkillCategory>()
            .Include(x => x.Skills)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        // Filter skills and order by profile sort order
        foreach (var category in categories)
        {
            var filteredSkills = category.Skills
                .Where(s => profileSkillIds.Contains(s.Id))
                .OrderBy(s => profileSkills.First(ps => ps.SkillId == s.Id).SortOrder)
                .ToList();
            category.Skills.Clear();
            foreach (var skill in filteredSkills)
            {
                category.Skills.Add(skill);
            }
        }

        // Remove empty categories
        categories = categories.Where(c => c.Skills.Count > 0).ToList();

        return (categories, highlightInfo);
    }

    private async Task<(List<WorkExperience> WorkExperiences, Dictionary<Guid, (bool IsHighlighted, string? DescriptionOverride)> Info)> GetWorkExperiencesForProfileAsync(
        Guid? profileId, CancellationToken cancellationToken)
    {
        if (!profileId.HasValue)
        {
            // No profile filtering - return all work experiences
            var allWorkExp = await dbContext.Set<WorkExperience>()
                .OrderBy(x => x.SortOrder)
                .ToListAsync(cancellationToken);
            return (allWorkExp, new Dictionary<Guid, (bool, string?)>());
        }

        // Get profile's work experiences with their info
        var profileWorkExps = await dbContext.Set<ProfileWorkExperience>()
            .Where(x => x.ProfileId == profileId.Value)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var workExpIds = profileWorkExps.Select(x => x.WorkExperienceId).ToList();
        var info = profileWorkExps.ToDictionary(
            x => x.WorkExperienceId,
            x => (x.IsHighlighted, x.DescriptionOverride));

        var workExperiences = await dbContext.Set<WorkExperience>()
            .Where(x => workExpIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        // Reorder by profile sort order
        workExperiences = workExperiences
            .OrderBy(w => workExpIds.IndexOf(w.Id))
            .ToList();

        return (workExperiences, info);
    }

    private async Task<(List<Project> Projects, Dictionary<Guid, (bool IsHighlighted, string? DescriptionOverride)> Info)> GetProjectsForProfileAsync(
        Guid? profileId, CancellationToken cancellationToken)
    {
        if (!profileId.HasValue)
        {
            // No profile filtering - return all projects sorted by timeline (newest first, current projects at top)
            var allProjects = await dbContext.Set<Project>()
                .Include(x => x.Technologies.OrderBy(t => t.SortOrder))
                .Include(x => x.Functions.OrderBy(f => f.SortOrder))
                .Include(x => x.TechnicalAspects.OrderBy(a => a.SortOrder))
                .Include(x => x.SubProjects.OrderBy(s => s.SortOrder))
                    .ThenInclude(s => s.Technologies.OrderBy(t => t.SortOrder))
                .OrderByDescending(x => x.IsCurrent)
                .ThenByDescending(x => x.StartDate)
                .ToListAsync(cancellationToken);
            return (allProjects, new Dictionary<Guid, (bool, string?)>());
        }

        // Get profile's projects with their info
        var profileProjects = await dbContext.Set<ProfileProject>()
            .Where(x => x.ProfileId == profileId.Value)
            .ToListAsync(cancellationToken);

        var projectIds = profileProjects.Select(x => x.ProjectId).ToHashSet();
        var info = profileProjects.ToDictionary(
            x => x.ProjectId,
            x => (x.IsHighlighted, x.DescriptionOverride));

        var projects = await dbContext.Set<Project>()
            .Include(x => x.Technologies.OrderBy(t => t.SortOrder))
            .Include(x => x.Functions.OrderBy(f => f.SortOrder))
            .Include(x => x.TechnicalAspects.OrderBy(a => a.SortOrder))
            .Include(x => x.SubProjects.OrderBy(s => s.SortOrder))
                .ThenInclude(s => s.Technologies.OrderBy(t => t.SortOrder))
            .Where(x => projectIds.Contains(x.Id))
            .OrderByDescending(x => x.IsCurrent)
            .ThenByDescending(x => x.StartDate)
            .ToListAsync(cancellationToken);

        return (projects, info);
    }

    private static PersonalDataDto MapPersonalData(PersonalData? entity)
    {
        if (entity is null)
        {
            return new PersonalDataDto(
                Name: "Unknown",
                Title: "",
                Email: "",
                Phone: "",
                Address: "",
                City: "",
                PostalCode: "",
                Country: "",
                BirthDate: DateOnly.MinValue,
                Citizenship: "",
                ProfileImageUrl: null
            );
        }

        return new PersonalDataDto(
            Name: entity.Name,
            Title: entity.Title,
            Email: entity.Email,
            Phone: entity.Phone,
            Address: entity.Address,
            City: entity.City,
            PostalCode: entity.PostalCode,
            Country: entity.Country,
            BirthDate: entity.BirthDate,
            Citizenship: entity.Citizenship,
            ProfileImageUrl: entity.ProfileImageUrl
        );
    }

    private static EducationDto MapEducation(Education entity)
    {
        return new EducationDto(
            Id: entity.Id,
            Institution: entity.Institution,
            Degree: entity.Degree,
            StartYear: entity.StartYear,
            EndYear: entity.EndYear,
            Description: entity.Description
        );
    }

    private static WorkExperienceDto MapWorkExperience(WorkExperience entity, Dictionary<Guid, (bool IsHighlighted, string? DescriptionOverride)> info)
    {
        var hasInfo = info.TryGetValue(entity.Id, out var profileInfo);
        return new WorkExperienceDto(
            Id: entity.Id,
            Company: entity.Company,
            Role: entity.Role,
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            Description: hasInfo && profileInfo.DescriptionOverride != null
                ? profileInfo.DescriptionOverride
                : entity.Description,
            IsCurrent: entity.IsCurrent,
            IsHighlighted: hasInfo && profileInfo.IsHighlighted
        );
    }

    private static SkillCategoryDto MapSkillCategory(SkillCategory entity, Dictionary<Guid, bool> highlightInfo)
    {
        return new SkillCategoryDto(
            Id: entity.Id,
            Name: entity.Name,
            Skills: entity.Skills.Select(s => new SkillDto(
                s.Id,
                s.Name,
                highlightInfo.TryGetValue(s.Id, out var isHighlighted) && isHighlighted
            )).ToList()
        );
    }

    private static ProjectDto MapProject(Project entity, Dictionary<Guid, (bool IsHighlighted, string? DescriptionOverride)> info)
    {
        var hasInfo = info.TryGetValue(entity.Id, out var profileInfo);
        return new ProjectDto(
            Id: entity.Id,
            Name: entity.Name,
            Description: hasInfo && profileInfo.DescriptionOverride != null
                ? profileInfo.DescriptionOverride
                : entity.Description,
            Framework: entity.Framework,
            Technologies: entity.Technologies.Select(t => t.Name).ToList(),
            Functions: entity.Functions.Select(f => f.Description).ToList(),
            TechnicalAspects: entity.TechnicalAspects.Select(a => a.Description).ToList(),
            SubProjects: entity.SubProjects.Select(MapSubProject).ToList(),
            AppStoreUrl: entity.AppStoreUrl,
            PlayStoreUrl: entity.PlayStoreUrl,
            AppGalleryUrl: entity.AppGalleryUrl,
            WebsiteUrl: entity.WebsiteUrl,
            ImageUrl: entity.ImageUrl,
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            IsCurrent: entity.IsCurrent,
            IsHighlighted: hasInfo && profileInfo.IsHighlighted
        );
    }

    private static SubProjectDto MapSubProject(ProjectSubProject entity)
    {
        return new SubProjectDto(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            Framework: entity.Framework,
            Technologies: entity.Technologies.Select(t => t.Name).ToList()
        );
    }

    private static FamilyMemberDto MapFamilyMember(FamilyMember entity)
    {
        return new FamilyMemberDto(
            Id: entity.Id,
            Relationship: entity.Relationship,
            Profession: entity.Profession,
            BirthYear: entity.BirthYear
        );
    }

    private static InternshipDto MapInternship(Internship entity)
    {
        return new InternshipDto(
            Id: entity.Id,
            Company: entity.Company,
            Role: entity.Role,
            Year: entity.Year,
            Month: entity.Month,
            EndMonth: entity.EndMonth,
            Description: entity.Description
        );
    }

    private static GitHubContributionsDto MapGitHubContributions(List<GitHubContribution> contributions)
    {
        return new GitHubContributionsDto(
            Username: "Codelisk",
            ProfileUrl: "https://github.com/Codelisk",
            TotalContributions: contributions.Sum(c => c.Count),
            Contributions: contributions.Select(c => new GitHubContributionDto(
                c.Date,
                c.Count,
                c.WeekNumber,
                c.DayOfWeek
            )).ToList()
        );
    }
}
