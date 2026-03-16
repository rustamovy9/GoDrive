using Domain.Common;

namespace Application.Filters;

public record UserFilter(
    string? UserName,
    string? FirstName,
    string? LastName,

    string? Email,
    string? PhoneNumber,
    string? Address,

    DateTimeOffset? MinDateOfBirth,
    DateTimeOffset? MaxDateOfBirth,

    int? RoleId,
    string? RoleName,

    bool? HasCars,
    bool? IsDeleted
) : BaseFilter;