using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class ProjectTechnicalAspect : BaseEntity
{
    public required string Description { get; set; }
    public int SortOrder { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
