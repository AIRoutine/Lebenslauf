using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class ProjectSubProjectTechnologyConfiguration : IEntityTypeConfiguration<ProjectSubProjectTechnology>
{
    public void Configure(EntityTypeBuilder<ProjectSubProjectTechnology> builder)
    {
        builder.ToTable("ProjectSubProjectTechnologies");

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        builder.HasIndex(x => x.SortOrder);
        builder.HasIndex(x => x.ProjectSubProjectId);
    }
}
