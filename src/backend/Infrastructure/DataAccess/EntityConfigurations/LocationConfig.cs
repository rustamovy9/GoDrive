using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class LocationConfig : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Latitude)
            .IsRequired();

        builder.Property(l => l.Longitude)
            .IsRequired();

        builder.HasIndex(l => new { l.Country, l.City }).IsUnique();

        builder.ToTable("Locations", t =>
        {
            t.HasCheckConstraint(
                "CK_Locations_Latitude_Range",
                "\"Latitude\" >= -90 AND \"Latitude\" <= 90"
            );

            t.HasCheckConstraint(
                "CK_Locations_Longitude_Range",
                "\"Longitude\" >= -180 AND \"Longitude\" <= 180"
            );
        });
    }
}