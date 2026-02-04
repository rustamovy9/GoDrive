using Domain.Common;

namespace Application.Filters;

public record RentalCompanyFilter(
    string? Search,
    string? Name,
    string? ContactInfo,

    int? OwnerId,
    int? CarId,

    bool? HasCars
) : BaseFilter;