using System.Text.Json;
using Lebenslauf.Api.Core.Data;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Handlers;

public class GetCvHandler(AppDbContext dbContext) : IRequestHandler<GetCvRequest, GetCvResponse>
{
    public async Task<GetCvResponse> Handle(GetCvRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        var personalData = await dbContext.Set<PersonalData>()
            .FirstOrDefaultAsync(cancellationToken);

        var education = await dbContext.Set<Education>()
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
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        return new GetCvResponse(
            PersonalData: MapPersonalData(personalData),
            Education: education.Select(MapEducation).ToList(),
            WorkExperience: workExperience.Select(MapWorkExperience).ToList(),
            SkillCategories: skillCategories.Select(MapSkillCategory).ToList(),
            Projects: projects.Select(MapProject).ToList()
        );
    }

    private static PersonalDataDto MapPersonalData(PersonalData? entity)
    {
        if (entity is null)
        {
            return new PersonalDataDto(
                Name: "Unknown",
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
        var technologies = new List<string>();
        if (!string.IsNullOrEmpty(entity.Technologies))
        {
            try
            {
                technologies = JsonSerializer.Deserialize<List<string>>(entity.Technologies) ?? [];
            }
            catch
            {
                // Ignore deserialization errors
            }
        }

        return new ProjectDto(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            Technologies: technologies,
            AppStoreUrl: entity.AppStoreUrl,
            PlayStoreUrl: entity.PlayStoreUrl,
            WebsiteUrl: entity.WebsiteUrl,
            ImageUrl: entity.ImageUrl
        );
    }
}
