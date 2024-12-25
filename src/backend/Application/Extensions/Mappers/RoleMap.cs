using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class RoleMap
{
    public static RoleReadInfo ToRead(this Role role)
    {
        return new()
        {
            Id = role.Id,
            Description = role.Description,
            Name = role.Name
        };
    }

    public static Role ToEntity(this RoleCreateInfo roleCreate)
    {
        return new Role()
        {
            Name = roleCreate.Name,
            Description = roleCreate.Description,
        };
    }

    public static Role ToEntity(this Role role, RoleUpdateInfo updateInfo)
    {
        role.Description = updateInfo.Description;
        role.Name = updateInfo.Name;
        role.Version++;
        role.UpdatedAt = DateTimeOffset.UtcNow;
        return role;
    }
}