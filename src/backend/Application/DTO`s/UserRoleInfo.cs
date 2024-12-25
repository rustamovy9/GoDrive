namespace Application.DTO_s;

public interface IBaseUserRoleInfo
{
    public int UserId { get; init; }
    public int RoleId { get; init; }
}

public readonly record struct UserRoleReadInfo(
    int Id,
    int UserId,
    int RoleId,
    UserReadInfo User,
    RoleReadInfo Role) : IBaseUserRoleInfo;

public readonly record struct UserRoleCreateInfo(
    int UserId,
    int RoleId) : IBaseUserRoleInfo;

public readonly record struct UserRoleUpdateInfo(
    int UserId,
    int RoleId) : IBaseUserRoleInfo;