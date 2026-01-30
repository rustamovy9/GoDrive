using Domain.Common;

namespace Domain.Entities;

public class Notification : BaseEntity
{
    public int UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }

    public User User { get; set; } = null!;
}