using Domain.Common;

namespace Application.Filters;

public record CarAvailabilityFilter(
    DateTimeOffset? DateFrom,
    DateTimeOffset? DateTo,
    bool? IsAvailable,
    int? LocationId
) : BaseFilter;