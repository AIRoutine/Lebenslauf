using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class FamilyMemberConfiguration : IEntityTypeConfiguration<FamilyMember>
{
    public void Configure(EntityTypeBuilder<FamilyMember> builder)
    {
        builder.ToTable("FamilyMembers");

        builder.Property(x => x.Relationship).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Profession).HasMaxLength(200).IsRequired();

        builder.HasIndex(x => x.SortOrder);
    }
}
