namespace Application.DTO_s;



public sealed record RentalCompanyReadInfo(
    int Id,
    string Name,
    string? ContactInfo,
    int OwnerId,
    int LocationId,
    string City,
    string County,
    DateTimeOffset CreatedAt);

public sealed record RentalCompanyUpdateInfo(
    string? Name,
    string? ContactInfo,
    int? LocationId);

public sealed record RentalCompanyCreateInfo(
    string Name,
    string? ContactInfo,
    int LocationId);
