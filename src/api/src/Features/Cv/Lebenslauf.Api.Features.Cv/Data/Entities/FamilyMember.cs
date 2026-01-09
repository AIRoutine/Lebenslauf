using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class FamilyMember : BaseEntity
{
    public required string Relationship { get; set; }
    public required string Profession { get; set; }
    public int? BirthYear { get; set; }
    public int SortOrder { get; set; }
}
