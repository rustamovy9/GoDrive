using Domain.Common;
using Domain.Constants;

namespace Domain.Entities;

public sealed class Car : BaseEntity
{
    public string Brand { get; set; } = null!; 
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string? Category { get; set; } 
    public string RegistrationNumber { get; set; } = null!; 
    public string Status { get; set; } = null!;
    public string? Location { get; set; }
    public string ImageCar { get; set; } = FileData.Default;
}
