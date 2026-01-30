namespace Application.DTO_s;

public sealed record CarAvailabilityReadInfo(
    int Id,
    int CarId,
    DateTimeOffset AvailableFrom,
    DateTimeOffset AvailableTo,
    bool IsAvailable);
    
public sealed record CarAvailabilityCreateInfo(
    int CarId,
    DateTimeOffset AvailableFrom,
    DateTimeOffset AvailableTo);
    
public sealed record CarAvailabilityUpdateInfo(
    DateTimeOffset? AvailableFrom,
    DateTimeOffset? AvailableTo);