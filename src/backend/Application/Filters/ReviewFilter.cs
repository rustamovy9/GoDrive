using Domain.Common;

namespace Application.Filters;

public record ReviewFilter(
    int? UserId,
    int? CarId,
    int? Rating,
    DateTimeOffset? FromDate,
    DateTimeOffset? ToDate) : BaseFilter;