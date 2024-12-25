using Domain.Common;

namespace Application.Filters;

public record UserFilter(
    string? UserName,
    string? FirstName,
    string? LastName,
    int? MinAge,
    int? MaxAge,
    DateTimeOffset? MinDateOfBirth,
    DateTimeOffset? MaxDateOfBirth,
    string? Email,
    string? PhoneNumber,
    string? Address) : BaseFilter;