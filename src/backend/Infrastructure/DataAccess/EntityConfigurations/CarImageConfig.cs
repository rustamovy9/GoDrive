using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class CarImageConfig : IEntityTypeConfiguration<CarImage>
{
    public void Configure(EntityTypeBuilder<CarImage> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .ValueGeneratedOnAdd();

        builder.Property(ci => ci.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(ci => ci.Car)
            .WithMany(c => c.CarImages)
            .HasForeignKey(ci => ci.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ci => ci.CarId);
    }
}