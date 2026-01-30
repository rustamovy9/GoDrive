namespace Application.DTO_s;



public sealed record RentalCompanyReadInfo(
    int Id,
    string Name,
    string? ContactInfo,
    int OwnerId,
    DateTimeOffset CreatedAt);

public sealed record RentalCompanyUpdateInfo(
    string? Name,
    string? ContactInfo);

public sealed record RentalCompanyCreateInfo(
    string Name,
    string? ContactInfo);
