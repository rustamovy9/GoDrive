using Domain.Common;

namespace Domain.Extensions;

public static class EntityExtension
{
    public static BaseEntity ToDelete(this BaseEntity entity)
    {
        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.IsDeleted = true;
        entity.DeletedAt = DateTimeOffset.UtcNow;
        return entity;
    }
}