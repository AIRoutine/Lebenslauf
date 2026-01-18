using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lebenslauf.Api.Features.Cv.Data.Configurations;

public class PersonalDataConfiguration : IEntityTypeConfiguration<PersonalData>
{
    public void Configure(EntityTypeBuilder<PersonalData> builder)
    {
        builder.ToTable("PersonalData");

        builder.Property(x => x.AcademicTitle).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(200).IsRequired();
        builder.Property(x => x.City).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PostalCode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Country).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Citizenship).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ProfileImageUrl).HasMaxLength(500);

        // Profile relationship
        builder.HasOne(x => x.Profile)
            .WithOne(x => x.PersonalData)
            .HasForeignKey<PersonalData>(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
