namespace Application.DTO_s;

public sealed record LocationReadInfo(
    int Id,
    string Country,
    string City,
    double Latitude,
    double Longitude);

public sealed record LocationCreatInfo(
    string Country,
    string City,
    double Latitude,
    double Longitude);

public sealed record LocationUpdateInfo(
    string? Country,
    string? City,
    double? Latitude,
    double? Longitude);