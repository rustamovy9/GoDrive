using Domain.Common;

namespace Application.Filters;

public record ReviewFilter(
    int? UserId,
    int? CarId,
    int? Rating,
    DateTimeOffset? ReviewDateStart,
    DateTimeOffset? ReviewDateEnd) : BaseFilter;