using Domain.Enums;

namespace Application.DTO_s;

public sealed record CarPriceReadInfo(
    int Id,
    int CarId,
    decimal PricePerDay,
    Currency Currency);
    
public sealed record CarPriceCreateInfo(
    int CarId,
    decimal PricePerDay,
    Currency Currency);
    
public sealed record CarPriceUpdateInfo(
    decimal? PricePerDay);