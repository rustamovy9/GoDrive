using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class RoleMap
{
    public static RoleReadInfo ToRead(this Role role)
    {
        return new RoleReadInfo
        (
            Id: role.Id,
            Name: role.Name,
            Description: role.Description
        );
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
        if (updateInfo.Description is not null)
            role.Description = updateInfo.Description;

        role.Version++;
        role.UpdatedAt = DateTimeOffset.UtcNow;

        return role;
    }
}