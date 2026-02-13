using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class CarPriceConfig : IEntityTypeConfiguration<CarPrice>
{
    public void Configure(EntityTypeBuilder<CarPrice> builder)
    {
        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Id)
            .ValueGeneratedOnAdd();

        builder.Property(cp => cp.PricePerDay)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(cp => cp.Currency)
            .IsRequired();

        builder.HasOne(cp => cp.Car)
            .WithMany()
            .HasForeignKey(cp => cp.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cp => cp.CarId).IsUnique();

        builder.ToTable("CarPrices", t =>
        {
            t.HasCheckConstraint(
                "CK_CarPrices_PricePerDay_Positive",
                "\"PricePerDay\" > 0"
            );
        });
    }
}