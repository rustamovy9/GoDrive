using Domain.Common;

namespace Application.Filters;

public record RentalCompanyFilter(
    string? Name,
    string? ContactInfo,
    string? City,
    string? Country, 
    
    int? OwnerId,
    int? LocationId,
    
    DateTimeOffset? CreatedFrom,
    DateTimeOffset? CreatedTo,

    bool? HasCars
) : BaseFilter;