namespace Application.Filters;

using Domain.Common;

public record RentalCompanyFilter(
    string? Name,              
    string? ContactInfo,        
    int? CarId                 
) : BaseFilter;
