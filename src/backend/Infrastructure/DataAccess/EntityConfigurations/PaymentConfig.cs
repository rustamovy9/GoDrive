using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class PaymentConfig : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.PaymentMethod)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.BookingId);
        builder.HasIndex(p => p.Status);

        builder.ToTable("Payments", t =>
        {
            t.HasCheckConstraint(
                "CK_Payments_Amount_Positive",
                "\"Amount\" > 0"
            );
        });
    }
}