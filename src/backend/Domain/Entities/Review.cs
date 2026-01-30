using Domain.Common;

namespace Domain.Entities;

public class Review : BaseEntity
{
    public int UserId { get; set; }
    public int CarId { get; set; } 
    public int Rating { get; set; }
    public string? Comment { get; set; } 
   
    public User User { get; set; } = null!;
    public Car Car { get; set; } = null!;
}
