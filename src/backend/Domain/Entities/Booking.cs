using Domain.Common;

namespace Domain.Entities;

public sealed class Booking : BaseEntity
{
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; } 
    public string? PickupLocation { get; set; } 
    public string? DropoffLocation { get; set; } 
    public string Status { get; set; } = null!; 
}
