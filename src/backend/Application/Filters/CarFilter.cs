using Domain.Common;
using Domain.Enums;

namespace Application.Filters;

public  record CarFilter(
    string? Brand,
    string? Model, 
    
    int? YearFrom,          
    int? YearTo,    
    
    int? CategoryId,    
    int? LocationId,
    int? OwnerId,
    
    string? RegistrationNumber,
    
    decimal? MinPrice,
    decimal? MaxPrice,
    
    CarStatus? Status
) : BaseFilter;