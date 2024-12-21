using Domain.Common;

namespace Domain.Entities;

public class Review : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int CarId { get; set; } 
    public Car Car { get; set; } = null!;
    public int Rating { get; set; }
    public string? Comment { get; set; } 
    public DateTime ReviewDate { get; set; } 
}
