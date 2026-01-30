namespace Application.DTO_s;


public sealed record UserRoleReadInfo(
    int Id,
    int UserId,
    int RoleId,
    UserReadInfo User,
    RoleReadInfo Role);

public sealed record UserRoleCreateInfo(
    int UserId,
    int RoleId);