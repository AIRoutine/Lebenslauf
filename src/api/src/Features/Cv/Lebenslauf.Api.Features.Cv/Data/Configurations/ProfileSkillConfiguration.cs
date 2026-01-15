using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class ProfileSkillConfiguration : IEntityTypeConfiguration<ProfileSkill>
{
    public void Configure(EntityTypeBuilder<ProfileSkill> builder)
    {
        builder.ToTable("ProfileSkills");

        // Composite primary key
        builder.HasKey(x => new { x.ProfileId, x.SkillId });

        // Profile relationship
        builder.HasOne(x => x.Profile)
            .WithMany(x => x.ProfileSkills)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Skill relationship
        builder.HasOne(x => x.Skill)
            .WithMany(x => x.ProfileSkills)
            .HasForeignKey(x => x.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
