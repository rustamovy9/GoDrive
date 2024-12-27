using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class BookingConfig : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(c => c.Status)
            .HasConversion<int>();
        
        builder.HasKey(b => b.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(b => b.PickupLocation).HasMaxLength(200);
        builder.Property(b => b.DropOffLocation).HasMaxLength(200);

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Car)
            .WithMany()
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}