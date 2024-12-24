using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class UserRoleMap
{
    public static UserRoleReadInfo ToRead(this UserRole userRole)
    {
        return new()
        {
            Id = userRole.Id,
            UserId = userRole.UserId,
            RoleId = userRole.RoleId,
            User = userRole.User.ToRead(),
            Role = userRole.Role.ToRead()
        };
    }

    public static UserRole ToEntity(this UserRoleCreateInfo userRoleCreate)
    {
        return new UserRole()
        {
            UserId = userRoleCreate.UserId,
            RoleId = userRoleCreate.RoleId
        };
    }

    public static UserRole ToEntity(this UserRole userRole, UserRoleUpdateInfo updateInfo)
    {
        userRole.UserId = updateInfo.UserId;
        userRole.RoleId = updateInfo.RoleId;
        userRole.Version++;
        userRole.UpdatedAt = DateTimeOffset.UtcNow;
        return userRole;
    }
}