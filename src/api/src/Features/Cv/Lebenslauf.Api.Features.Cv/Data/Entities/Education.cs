using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class Education : BaseEntity
{
    public required string Institution { get; set; }
    public required string Degree { get; set; }
    public required int StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}
