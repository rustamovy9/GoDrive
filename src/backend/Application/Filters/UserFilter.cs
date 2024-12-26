using Domain.Common;

namespace Application.Filters;

public record UserFilter(
    string? UserName,
    string? FirstName,
    string? LastName,
    DateTimeOffset? MinDateOfBirth,
    DateTimeOffset? MaxDateOfBirth,
    string? Email,
    string? PhoneNumber,
    string? Address) : BaseFilter;