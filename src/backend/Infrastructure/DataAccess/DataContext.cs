using Domain.Entities;
using Domain.Enums;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    // 👤 Users & Roles
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    // 🚗 Cars
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarDocument> CarDocuments { get; set; }
    public DbSet<CarImage> CarImages { get; set; }
    public DbSet<CarPrice> CarPrices { get; set; }
    
    //Categories
    public DbSet<Category> Categories  { get; set; }
    
    // 🔔 Notifications
    public DbSet<Notification> Notifications  { get; set; }

    // 🏢 Company
    public DbSet<RentalCompany> RentalCompanies { get; set; }
    
    // 📍 Location
    public DbSet<Location> Locations  { get; set; }
    
    // 📅 Booking & Payment
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments  { get; set; } 
    
    // ⭐ Reviews
    public DbSet<Review> Reviews { get; set; }
    
    // AI
    public DbSet<ChatMessage> ChatMessages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        modelBuilder.FilterSoftDeletedProperties();
    }
}
