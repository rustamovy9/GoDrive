using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class CarDocumentConfig : IEntityTypeConfiguration<CarDocument>
{
    public void Configure(EntityTypeBuilder<CarDocument> builder)
    {
        builder.HasKey(cd => cd.Id);

        builder.Property(cd => cd.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(cd => cd.Car)
            .WithMany()
            .HasForeignKey(cd => cd.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(cd => cd.DocumentType)
            .IsRequired();

        builder.Property(cd => cd.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(cd => cd.VerificationStatus)
            .IsRequired();

        builder.HasIndex(cd => cd.CarId);
        builder.HasIndex(cd => new { cd.CarId, cd.DocumentType });

        builder.HasIndex(cd => new { cd.CarId, cd.DocumentType })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasOne(d => d.VerifiedByAdmin)
            .WithMany()
            .HasForeignKey(d => d.VerifiedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}