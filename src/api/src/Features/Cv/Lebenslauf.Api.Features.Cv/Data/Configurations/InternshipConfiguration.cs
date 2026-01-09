using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class InternshipConfiguration : IEntityTypeConfiguration<Internship>
{
    public void Configure(EntityTypeBuilder<Internship> builder)
    {
        builder.ToTable("Internships");

        builder.Property(x => x.Company).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Role).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasIndex(x => x.SortOrder);
        builder.HasIndex(x => x.Year);
    }
}
