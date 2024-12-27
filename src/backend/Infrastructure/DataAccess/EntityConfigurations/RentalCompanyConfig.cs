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

        builder.Property(rc => rc.Name).IsRequired().HasMaxLength(200);
        builder.Property(rc => rc.ContactInfo).HasMaxLength(500);
        
    }
}