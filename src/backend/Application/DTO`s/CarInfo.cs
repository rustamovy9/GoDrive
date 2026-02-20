using Domain.Enums;

namespace Application.DTO_s;
public sealed record CarReadInfo(
    int Id,
    string Brand,
    string Model,
    int Year,
    CarStatus CarStatus,
    
    int CategoryId,
    int LocationId,
    int? RentalCompanyId,
    
    IReadOnlyList<string> Images,
    
    decimal CurrentPricePerDay,
    
    DateTimeOffset CreatedAt);

public record CarDetailReadInfo(
    int Id,
    string Brand,
    string Model,
    int Year,
    string RegistrationNumber,
    CarStatus Status,

    string CategoryName,
    string Location,
    string OwnerName,

    decimal CurrentPricePerDay,

    IReadOnlyList<string> Images,

    IReadOnlyList<CarDocumentReadInfo> Documents,

    DateTimeOffset CreatedAt
);



public sealed record CarUpdateInfo(
    string? Brand,
    string? Model,
    int? Year,
    
    int? CategoryId,
    int? LocationId,
    int? RentalCompanyId);

public sealed record CarCreateInfo(
    string Brand,
    string Model,
    int Year,
    string RegistrationNumber,
    
    int CategoryId,
    int LocationId,
    int? RentalCompanyId);


public sealed record CarUpdateStatusInfo(
    CarStatus Status
);