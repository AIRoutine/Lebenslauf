using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class GitHubContributionConfiguration : IEntityTypeConfiguration<GitHubContribution>
{
    public void Configure(EntityTypeBuilder<GitHubContribution> builder)
    {
        builder.ToTable("GitHubContributions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Count)
            .IsRequired();

        builder.Property(x => x.WeekNumber)
            .IsRequired();

        builder.Property(x => x.DayOfWeek)
            .IsRequired();

        // Index for efficient date range queries
        builder.HasIndex(x => x.Date);
        builder.HasIndex(x => new { x.WeekNumber, x.DayOfWeek });
    }
}
