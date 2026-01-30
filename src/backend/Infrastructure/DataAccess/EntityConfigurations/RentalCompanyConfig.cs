using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class RentalCompanyConfig : IEntityTypeConfiguration<RentalCompany>
{
    public void Configure(EntityTypeBuilder<RentalCompany> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        
        builder.Property(rc => rc.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(rc => rc.Name);
        
        builder.HasIndex(rc => new {rc.OwnerId ,rc.Name}).IsUnique();
        
        builder.Property(rc => rc.ContactInfo)
            .HasMaxLength(500);

        builder.HasOne(rc => rc.Owner)
            .WithMany()
            .HasForeignKey(rc=>rc.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(rc => rc.Cars)
            .WithOne(c => c.RentalCompany)
            .HasForeignKey(c => c.RentalCompanyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}