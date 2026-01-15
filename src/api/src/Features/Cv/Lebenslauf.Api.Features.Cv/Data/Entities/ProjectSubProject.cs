using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class ProjectSubProject : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Framework { get; set; }
    public int SortOrder { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public ICollection<ProjectSubProjectTechnology> Technologies { get; set; } = [];
}
