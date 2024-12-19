namespace Domain.Models;

public class BaseEntity
{
    public int  Id { get; set; }
    public DateTime DeletedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}