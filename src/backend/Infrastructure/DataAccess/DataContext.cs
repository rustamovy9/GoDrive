using Backend.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class DataContext:DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<RentalCompany> RentalCompanies { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}