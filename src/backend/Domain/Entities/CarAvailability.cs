using Domain.Common;

namespace Domain.Entities;

public class CarAvailability : BaseEntity
{
    public int CarId { get; set; }
    public DateTimeOffset AvailableFrom { get; set; }
    public DateTimeOffset AvailableTo { get; set; }
    public bool IsAvailable { get; set; }

    public Car Car { get; set; } = null!;
}