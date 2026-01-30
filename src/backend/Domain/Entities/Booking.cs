using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Booking : BaseEntity
{
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; } 
    
    public bool IsContactShared { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public BookingStatus BookingStatus { get; set; }
    
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? Comment { get; set; }
    
    public int UserId { get; set; } 
    public int CarId { get; set; }
    public int PickupLocationId { get; set; }
    public int DropOffLocationId { get; set; } 
    
    
    public Location PickupLocation { get; set; } = null!; 
    public Location DropOffLocation { get; set; } = null!; 
    public User User { get; set; } = null!;
    public Car Car { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = [];
}
