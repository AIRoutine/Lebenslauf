using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class WorkExperience : BaseEntity
{
    public required string Company { get; set; }
    public required string Role { get; set; }
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public bool IsCurrent { get; set; }
    public int SortOrder { get; set; }
}
