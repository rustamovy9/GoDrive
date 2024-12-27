using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class CarConfig : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.Property(c => c.CarStatus)
            .HasConversion<int>();
        
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Brand).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Model).IsRequired().HasMaxLength(100);
        builder.Property(c => c.RegistrationNumber).IsRequired().HasMaxLength(50);
        builder.Property(c => c.ImageCar).IsRequired();

        builder.HasOne<RentalCompany>()
            .WithMany(r => r.Cars)
            .HasForeignKey("RentalCompanyId")
            .OnDelete(DeleteBehavior.SetNull);
    }
}