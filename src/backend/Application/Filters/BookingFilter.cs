using Domain.Common;
using Domain.Enums;

namespace Application.Filters;

public record BookingFilter(
    int? UserId,
    int? CarId,
    Status? Status,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate) : BaseFilter;