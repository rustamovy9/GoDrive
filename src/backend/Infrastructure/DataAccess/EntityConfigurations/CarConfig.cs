using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class CarConfig : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Brand)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.Model)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.Year)
            .IsRequired();
        
        builder.Property(c => c.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(c => c.RegistrationNumber)
            .IsUnique();
        
        builder.Property(c => c.CarStatus)
            .IsRequired();

        builder.HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Owner)
            .WithMany(u => u.OwnedCars)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c=>c.RentalCompany)
            .WithMany(rc => rc.Cars)
            .HasForeignKey(c=>c.RentalCompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Location)
            .WithMany()
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c=>c.Bookings)
            .WithOne(b=>b.Car)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Reviews)
            .WithOne(r => r.Car)
            .HasForeignKey(r => r.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(c => c.CarImages)
            .WithOne(ci => ci.Car)
            .HasForeignKey(ci => ci.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}