using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class ProjectSubProjectConfiguration : IEntityTypeConfiguration<ProjectSubProject>
{
    public void Configure(EntityTypeBuilder<ProjectSubProject> builder)
    {
        builder.ToTable("ProjectSubProjects");

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Framework).HasMaxLength(100);

        builder.HasIndex(x => x.SortOrder);
        builder.HasIndex(x => x.ProjectId);

        // Navigation properties
        builder.HasMany(x => x.Technologies)
            .WithOne(x => x.ProjectSubProject)
            .HasForeignKey(x => x.ProjectSubProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
