namespace Domain.Common;

public abstract class BaseEntity
{
    public int  Id { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset DeletedAt { get; set; } = DateTimeOffset.MinValue;
    public bool IsDeleted { get; set; }
    public long Version { get; set; } = 1;
    public bool IsActive { get; set; } = true;
}