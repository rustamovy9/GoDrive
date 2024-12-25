namespace Application.DTO_s;

public interface IBaseRoleInfo
{
    public string Name { get; init; }
    public string? Description { get; init; }
}

public readonly record struct RoleReadInfo(
    int Id,
    string Name,
    string? Description) : IBaseRoleInfo;

public readonly record struct RoleCreateInfo(
    string Name,
    string? Description) : IBaseRoleInfo;

public readonly record struct RoleUpdateInfo(
    string Name,
    string? Description) : IBaseRoleInfo;