using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class Project : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string Technologies { get; set; } = string.Empty; // JSON array stored as string
    public string? AppStoreUrl { get; set; }
    public string? PlayStoreUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}
