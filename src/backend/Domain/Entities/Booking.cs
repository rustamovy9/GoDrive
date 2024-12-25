using Domain.Common;
using Domain.Enums;

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
    public string? DropOffLocation { get; set; } 
    public Status Status { get; set; }
}
