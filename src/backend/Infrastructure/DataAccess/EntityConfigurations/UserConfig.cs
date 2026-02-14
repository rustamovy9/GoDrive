using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(u => u.UserName)
            .IsUnique();
        
        builder.Property(u => u.Email)
            .IsRequired().
            HasMaxLength(200);

        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(u => u.AvatarPath).IsRequired().HasMaxLength(500);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(u => u.Address)
            .HasMaxLength(300);

        builder.Property(u => u.DateOfBirth)
            .IsRequired();

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.OwnedCars)
            .WithOne(c => c.Owner)
            .HasForeignKey(с => с.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}