using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class Project : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Framework { get; set; }
    public string? AppStoreUrl { get; set; }
    public string? PlayStoreUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }

    // Navigation properties (normalized)
    public ICollection<ProjectTechnology> Technologies { get; set; } = [];
    public ICollection<ProjectFunction> Functions { get; set; } = [];
    public ICollection<ProjectTechnicalAspect> TechnicalAspects { get; set; } = [];
}
