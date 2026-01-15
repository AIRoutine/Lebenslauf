using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class ProfileProjectConfiguration : IEntityTypeConfiguration<ProfileProject>
{
    public void Configure(EntityTypeBuilder<ProfileProject> builder)
    {
        builder.ToTable("ProfileProjects");

        // Composite primary key
        builder.HasKey(x => new { x.ProfileId, x.ProjectId });

        // Profile relationship
        builder.HasOne(x => x.Profile)
            .WithMany(x => x.ProfileProjects)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project relationship
        builder.HasOne(x => x.Project)
            .WithMany(x => x.ProfileProjects)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // DescriptionOverride can be long
        builder.Property(x => x.DescriptionOverride)
            .HasMaxLength(2000);
    }
}
