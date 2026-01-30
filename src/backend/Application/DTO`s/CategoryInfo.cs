namespace Application.DTO_s;

public sealed record CategoryReadInfo(
    int Id,
    string Name);
public sealed record CategoryCreateInfo(string Name);
public sealed record CategoryUpdateInfo(string? Name);