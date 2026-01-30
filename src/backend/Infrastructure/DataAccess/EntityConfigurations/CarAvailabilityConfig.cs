using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class CarAvailabilityConfig : IEntityTypeConfiguration<CarAvailability>
{
    public void Configure(EntityTypeBuilder<CarAvailability> builder)
    {
        builder.HasKey(ca => ca.Id);

        builder.Property(ca => ca.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(ca => ca.Car)
            .WithMany()
            .HasForeignKey(ca => ca.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ca => ca.AvailableFrom)
            .IsRequired();

        builder.Property(ca => ca.AvailableTo)
            .IsRequired();

        builder.Property(ca => ca.IsAvailable)
            .IsRequired();

        builder.HasIndex(ca => ca.CarId);

        builder.HasIndex(ca => new { ca.CarId, ca.AvailableFrom, ca.AvailableTo });

        builder.ToTable("CarAvailabilities", t =>
        {
            t.HasCheckConstraint(
                "CK_CarAvailability_DateRange",
                "\"AvailableFrom\" < \"AvailableTo\""
            );
        });
    }
}