using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class SkillCategory : BaseEntity
{
    public required string Name { get; set; }
    public int SortOrder { get; set; }

    public ICollection<Skill> Skills { get; set; } = [];
}
