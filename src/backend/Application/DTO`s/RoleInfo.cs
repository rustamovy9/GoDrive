namespace Application.DTO_s;
public sealed record RoleReadInfo(
    int Id,
    string Name,
    string? Description);

public sealed record RoleCreateInfo(
    string Name,
    string? Description);

public sealed record RoleUpdateInfo(
    string? Name,
    string? Description);