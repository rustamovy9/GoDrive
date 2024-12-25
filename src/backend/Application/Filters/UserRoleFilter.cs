using Domain.Common;

namespace Application.Filters;

public record UserRoleFilter(
    int? UserId,
    int? RoleId) : BaseFilter;
