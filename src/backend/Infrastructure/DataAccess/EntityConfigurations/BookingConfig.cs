using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class BookingConfig : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.Property(b => b.StartDateTime)
            .IsRequired();
        
        builder.Property(b => b.EndDateTime)
            .IsRequired();
        
        builder.Property(b => b.TotalPrice)
            .IsRequired()
            .HasPrecision(18,2);
        
        builder.Property(b => b.BookingStatus)
            .IsRequired();
        
        builder.Property(b => b.PaymentMethod)
            .IsRequired(); 
        
        builder.Property(b => b.PaymentStatus)
            .IsRequired();
        
        builder.Property(b => b.Comment)
            .HasMaxLength(500);

        builder.HasOne(b => b.User)
            .WithMany(u=>u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Car)
            .WithMany(c=>c.Bookings)
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(b => b.PickupLocation)
            .WithMany()
            .HasForeignKey(b => b.PickupLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.DropOffLocation)
            .WithMany()
            .HasForeignKey(b => b.DropOffLocationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(b => b.PaymentMethod)
            .HasDefaultValue(PaymentMethod.Offline)
            .IsRequired();

        builder.HasMany(b => b.Payments)
            .WithOne(p => p.Booking)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(b => b.IsContactShared)
            .IsRequired();
        
        builder.ToTable("Bookings", t =>
        {
            t.HasCheckConstraint(
                "CK_Bookings_PaymentMethod_Offline",
                "\"PaymentMethod\" = 0"
            );
        });
    }
}