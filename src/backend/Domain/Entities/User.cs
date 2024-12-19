using Domain.Models;

namespace Backend.Models;

public class User : BaseEntity
{
    public string FirstName { get; set; } = null!; 
    public string LastName { get; set; } = null!; 
    public string Email { get; set; } = null!; 
    public string? PhoneNumber { get; set; } 
    public string? Address { get; set; } 
    public string? DriverLicense { get; set; } 
}
