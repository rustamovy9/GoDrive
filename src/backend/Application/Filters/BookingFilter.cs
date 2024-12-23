using Domain.Common;

namespace Application.Filters;

public record BookingFilter(
    int? UserId,
    int? CarId,
    string? Status,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate) : BaseFilter;