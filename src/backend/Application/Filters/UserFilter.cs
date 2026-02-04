using Domain.Common;

namespace Application.Filters;

public record UserFilter(
    string? Search,

    string? UserName,
    string? FirstName,
    string? LastName,

    string? Email,
    string? PhoneNumber,
    string? Address,

    DateTimeOffset? MinDateOfBirth,
    DateTimeOffset? MaxDateOfBirth,

    int? RoleId,

    bool? HasCars,
    bool? HasDriverLicense,
    bool? IsDeleted
) : BaseFilter;