using Domain.Common;
using Domain.Constants;
using Domain.Enums;

namespace Domain.Entities;

public class Car : BaseEntity
{
    public string Brand { get; set; } = null!; 
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public CarStatus CarStatus { get; set; }

    public int CategoryId { get; set; }  
    public int OwnerId { get; set; }
    public int? RentalCompanyId { get; set; }
    public int LocationId { get; set; }

    public User Owner { get; set; } = null!;
    public Location Location { get; set; } = null!;
    public RentalCompany? RentalCompany { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<CarImage> CarImages { get; set; } = [];
}
