using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class ProjectFunctionConfiguration : IEntityTypeConfiguration<ProjectFunction>
{
    public void Configure(EntityTypeBuilder<ProjectFunction> builder)
    {
        builder.ToTable("ProjectFunctions");

        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();

        builder.HasIndex(x => x.SortOrder);
        builder.HasIndex(x => x.ProjectId);
    }
}
