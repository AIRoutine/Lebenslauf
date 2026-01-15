using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class ProfileWorkExperienceConfiguration : IEntityTypeConfiguration<ProfileWorkExperience>
{
    public void Configure(EntityTypeBuilder<ProfileWorkExperience> builder)
    {
        builder.ToTable("ProfileWorkExperiences");

        // Composite primary key
        builder.HasKey(x => new { x.ProfileId, x.WorkExperienceId });

        // Profile relationship
        builder.HasOne(x => x.Profile)
            .WithMany(x => x.ProfileWorkExperiences)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // WorkExperience relationship
        builder.HasOne(x => x.WorkExperience)
            .WithMany(x => x.ProfileWorkExperiences)
            .HasForeignKey(x => x.WorkExperienceId)
            .OnDelete(DeleteBehavior.Cascade);

        // DescriptionOverride can be long
        builder.Property(x => x.DescriptionOverride)
            .HasMaxLength(2000);
    }
}
