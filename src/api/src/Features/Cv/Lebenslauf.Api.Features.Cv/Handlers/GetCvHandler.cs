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
    [MediatorHttpGet("/api/cv", OperationId = "GetCv")]
    public async Task<GetCvResponse> Handle(GetCvRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        var personalData = await dbContext.Set<PersonalData>()
            .FirstOrDefaultAsync(cancellationToken);

        var familyMembers = await dbContext.Set<FamilyMember>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var education = await dbContext.Set<Education>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var internships = await dbContext.Set<Internship>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var workExperience = await dbContext.Set<WorkExperience>()
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var skillCategories = await dbContext.Set<SkillCategory>()
            .Include(x => x.Skills.OrderBy(s => s.SortOrder))
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var projects = await dbContext.Set<Project>()
            .Include(x => x.Technologies.OrderBy(t => t.SortOrder))
            .Include(x => x.Functions.OrderBy(f => f.SortOrder))
            .Include(x => x.TechnicalAspects.OrderBy(a => a.SortOrder))
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var gitHubContributions = await dbContext.Set<GitHubContribution>()
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);

        return new GetCvResponse(
            PersonalData: MapPersonalData(personalData),
            FamilyMembers: familyMembers.Select(MapFamilyMember).ToList(),
            Education: education.Select(MapEducation).ToList(),
            Internships: internships.Select(MapInternship).ToList(),
            WorkExperience: workExperience.Select(MapWorkExperience).ToList(),
            SkillCategories: skillCategories.Select(MapSkillCategory).ToList(),
            Projects: projects.Select(MapProject).ToList(),
            GitHub: MapGitHubContributions(gitHubContributions)
        );
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

    private static WorkExperienceDto MapWorkExperience(WorkExperience entity)
    {
        return new WorkExperienceDto(
            Id: entity.Id,
            Company: entity.Company,
            Role: entity.Role,
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            Description: entity.Description,
            IsCurrent: entity.IsCurrent
        );
    }

    private static SkillCategoryDto MapSkillCategory(SkillCategory entity)
    {
        return new SkillCategoryDto(
            Id: entity.Id,
            Name: entity.Name,
            Skills: entity.Skills.Select(s => new SkillDto(s.Id, s.Name)).ToList()
        );
    }

    private static ProjectDto MapProject(Project entity)
    {
        return new ProjectDto(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            Framework: entity.Framework,
            Technologies: entity.Technologies.Select(t => t.Name).ToList(),
            Functions: entity.Functions.Select(f => f.Description).ToList(),
            TechnicalAspects: entity.TechnicalAspects.Select(a => a.Description).ToList(),
            AppStoreUrl: entity.AppStoreUrl,
            PlayStoreUrl: entity.PlayStoreUrl,
            WebsiteUrl: entity.WebsiteUrl,
            ImageUrl: entity.ImageUrl
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
