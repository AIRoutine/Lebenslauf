using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class Skill : BaseEntity
{
    public required string Name { get; set; }
    public int SortOrder { get; set; }

    public Guid CategoryId { get; set; }
    public SkillCategory Category { get; set; } = null!;

    // M:N relationship with profiles
    public ICollection<ProfileSkill> ProfileSkills { get; set; } = [];
}
