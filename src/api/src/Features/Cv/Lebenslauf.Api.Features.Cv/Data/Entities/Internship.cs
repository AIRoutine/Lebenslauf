using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class Internship : BaseEntity
{
    public required string Company { get; set; }
    public required string Role { get; set; }
    public required int Year { get; set; }
    public int? Month { get; set; }
    public int? EndMonth { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}
