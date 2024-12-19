namespace Backend.Models;

public class Car : BaseEntity
{
    public string Brand { get; set; } = null!; 
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string? Category { get; set; } 
    public string RegistrationNumber { get; set; } = null!; 
    public string Status { get; set; } = null!;
    public string? Location { get; set; } 
}
