using Domain.Common;
using Domain.Constants;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!; 
    public string LastName { get; set; } = null!;
    public DateTimeOffset DateOfBirth { get; set; }
    public string Email { get; set; } = null!; 
    public string? PhoneNumber { get; set; } 
    public string? Address { get; set; } 
    public string? DriverLicense { get; set; }
    public string AvatarPath { get; set; } = FileData.Default;
    public string PasswordHash { get; set; } = null!;
    
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Car>? OwnedCars { get; set; } = [];
}
