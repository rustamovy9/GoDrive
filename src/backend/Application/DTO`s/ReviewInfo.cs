namespace Application.DTO_s;


public sealed record ReviewReadInfo(
    int Id,
    int CarId,
    int Rating,
    string? Comment,
    string UserName,
    DateTimeOffset CreatedAt);

public sealed record ReviewUpdateInfo(
    string? Comment);

public sealed record ReviewCreateInfo(
    int CarId,
    int Rating,
    string? Comment);