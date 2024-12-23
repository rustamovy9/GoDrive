using System.Reflection;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.DataAccess;

public class DataContext (DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<RentalCompany> RentalCompanies { get; set; }
    public DbSet<Review> Reviews { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        

        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        modelBuilder.FilterSoftDeletedProperties();
    }
}

